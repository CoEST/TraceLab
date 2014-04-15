/*
 * Copyright (c) 2000, 2010, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

package sun.nio.ch;

import java.lang.ref.SoftReference;
import java.lang.reflect.*;
import java.io.IOException;
import java.io.FileDescriptor;
import java.nio.ByteBuffer;
import java.nio.MappedByteBuffer;
import java.nio.channels.*;
import java.security.AccessController;
import java.security.PrivilegedAction;
import java.util.*;
import sun.misc.Unsafe;
import sun.misc.Cleaner;
import sun.security.action.GetPropertyAction;


class Util {

    private static class SelectorWrapper {
        private Selector sel;
        private SelectorWrapper (Selector sel) {
            this.sel = sel;
            Cleaner.create(this, new Closer(sel));
        }
        private static class Closer implements Runnable {
            private Selector sel;
            private Closer (Selector sel) {
                this.sel = sel;
            }
            public void run () {
                try {
                    sel.close();
                } catch (Throwable th) {
                    throw new Error(th);
                }
            }
        }
        public Selector get() { return sel;}
    }

    // Per-thread cached selector
    private static ThreadLocal<SoftReference<SelectorWrapper>> localSelector
        = new ThreadLocal<SoftReference<SelectorWrapper>>();
    // Hold a reference to the selWrapper object to prevent it from
    // being cleaned when the temporary selector wrapped is on lease.
    private static ThreadLocal<SelectorWrapper> localSelectorWrapper
        = new ThreadLocal<SelectorWrapper>();

    // When finished, invoker must ensure that selector is empty
    // by cancelling any related keys and explicitly releasing
    // the selector by invoking releaseTemporarySelector()
    static Selector getTemporarySelector(SelectableChannel sc)
        throws IOException
    {
        SoftReference<SelectorWrapper> ref = localSelector.get();
        SelectorWrapper selWrapper = null;
        Selector sel = null;
        if (ref == null
            || ((selWrapper = ref.get()) == null)
            || ((sel = selWrapper.get()) == null)
            || (sel.provider() != sc.provider())) {
            sel = sc.provider().openSelector();
            selWrapper = new SelectorWrapper(sel);
            localSelector.set(new SoftReference<SelectorWrapper>(selWrapper));
        }
        localSelectorWrapper.set(selWrapper);
        return sel;
    }

    static void releaseTemporarySelector(Selector sel)
        throws IOException
    {
        // Selector should be empty
        sel.selectNow();                // Flush cancelled keys
        assert sel.keys().isEmpty() : "Temporary selector not empty";
        localSelectorWrapper.set(null);
    }


    // -- Random stuff --

    static ByteBuffer[] subsequence(ByteBuffer[] bs, int offset, int length) {
        if ((offset == 0) && (length == bs.length))
            return bs;
        int n = length;
        ByteBuffer[] bs2 = new ByteBuffer[n];
        for (int i = 0; i < n; i++)
            bs2[i] = bs[offset + i];
        return bs2;
    }

    static <E> Set<E> ungrowableSet(final Set<E> s) {
        return new Set<E>() {

                public int size()                 { return s.size(); }
                public boolean isEmpty()          { return s.isEmpty(); }
                public boolean contains(Object o) { return s.contains(o); }
                public Object[] toArray()         { return s.toArray(); }
                public <T> T[] toArray(T[] a)     { return s.toArray(a); }
                public String toString()          { return s.toString(); }
                public Iterator<E> iterator()     { return s.iterator(); }
                public boolean equals(Object o)   { return s.equals(o); }
                public int hashCode()             { return s.hashCode(); }
                public void clear()               { s.clear(); }
                public boolean remove(Object o)   { return s.remove(o); }

                public boolean containsAll(Collection<?> coll) {
                    return s.containsAll(coll);
                }
                public boolean removeAll(Collection<?> coll) {
                    return s.removeAll(coll);
                }
                public boolean retainAll(Collection<?> coll) {
                    return s.retainAll(coll);
                }

                public boolean add(E o){
                    throw new UnsupportedOperationException();
                }
                public boolean addAll(Collection<? extends E> coll) {
                    throw new UnsupportedOperationException();
                }

        };
    }


    private static volatile Constructor directByteBufferConstructor = null;

    private static void initDBBConstructor() {
        AccessController.doPrivileged(new PrivilegedAction<Void>() {
                public Void run() {
                    try {
                        Class<?> cl = Class.forName("java.nio.DirectByteBuffer");
                        Constructor ctor = cl.getDeclaredConstructor(
                            new Class[] { int.class,
                                          long.class,
                                          FileDescriptor.class,
                                          Runnable.class });
                        ctor.setAccessible(true);
                        directByteBufferConstructor = ctor;
                    } catch (ClassNotFoundException x) {
                        throw new InternalError();
                    } catch (NoSuchMethodException x) {
                        throw new InternalError();
                    } catch (IllegalArgumentException x) {
                        throw new InternalError();
                    } catch (ClassCastException x) {
                        throw new InternalError();
                    }
                    return null;
                }});
    }

    static MappedByteBuffer newMappedByteBuffer(int size, long addr,
                                                FileDescriptor fd,
                                                Runnable unmapper)
    {
        MappedByteBuffer dbb;
        if (directByteBufferConstructor == null)
            initDBBConstructor();
        try {
            dbb = (MappedByteBuffer)directByteBufferConstructor.newInstance(
              new Object[] { new Integer(size),
                             new Long(addr),
                             fd,
                             unmapper });
        } catch (InstantiationException e) {
            throw new InternalError();
        } catch (IllegalAccessException e) {
            throw new InternalError();
        } catch (InvocationTargetException e) {
            throw new InternalError();
        }
        return dbb;
    }

    private static volatile Constructor directByteBufferRConstructor = null;

    private static void initDBBRConstructor() {
        AccessController.doPrivileged(new PrivilegedAction<Void>() {
                public Void run() {
                    try {
                        Class<?> cl = Class.forName("java.nio.DirectByteBufferR");
                        Constructor ctor = cl.getDeclaredConstructor(
                            new Class[] { int.class,
                                          long.class,
                                          FileDescriptor.class,
                                          Runnable.class });
                        ctor.setAccessible(true);
                        directByteBufferRConstructor = ctor;
                    } catch (ClassNotFoundException x) {
                        throw new InternalError();
                    } catch (NoSuchMethodException x) {
                        throw new InternalError();
                    } catch (IllegalArgumentException x) {
                        throw new InternalError();
                    } catch (ClassCastException x) {
                        throw new InternalError();
                    }
                    return null;
                }});
    }

    static MappedByteBuffer newMappedByteBufferR(int size, long addr,
                                                 FileDescriptor fd,
                                                 Runnable unmapper)
    {
        MappedByteBuffer dbb;
        if (directByteBufferRConstructor == null)
            initDBBRConstructor();
        try {
            dbb = (MappedByteBuffer)directByteBufferRConstructor.newInstance(
              new Object[] { new Integer(size),
                             new Long(addr),
                             fd,
                             unmapper });
        } catch (InstantiationException e) {
            throw new InternalError();
        } catch (IllegalAccessException e) {
            throw new InternalError();
        } catch (InvocationTargetException e) {
            throw new InternalError();
        }
        return dbb;
    }


    // -- Bug compatibility --

    private static volatile String bugLevel = null;

    static boolean atBugLevel(String bl) {              // package-private
        if (bugLevel == null) {
            if (!sun.misc.VM.isBooted())
                return false;
            String value = AccessController.doPrivileged(
                new GetPropertyAction("sun.nio.ch.bugLevel"));
            bugLevel = (value != null) ? value : "";
        }
        return bugLevel.equals(bl);
    }



    // -- Initialization --


    static void load() {
    }

}
