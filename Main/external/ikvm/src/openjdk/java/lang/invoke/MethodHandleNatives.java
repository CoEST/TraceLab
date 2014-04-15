/*
 * Copyright (c) 2008, 2011, Oracle and/or its affiliates. All rights reserved.
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

/*
 * Extensively modified for IKVM.NET by Jeroen Frijters
 * Copyright (C) 2011 Jeroen Frijters
 */

package java.lang.invoke;

import java.lang.invoke.MethodHandles.Lookup;
import java.lang.reflect.AccessibleObject;
import java.lang.reflect.Field;
import static java.lang.invoke.MethodHandleNatives.Constants.*;
import static java.lang.invoke.MethodHandles.Lookup.IMPL_LOOKUP;

/**
 * The JVM interface for the method handles package is all here.
 * This is an interface internal and private to an implemetantion of JSR 292.
 * <em>This class is not part of the JSR 292 standard.</em>
 * @author jrose
 */
class MethodHandleNatives {

    private MethodHandleNatives() { } // static only

    /// MethodName support

    static native void init(MemberName self, Object ref);
    static native void expand(MemberName self);
    static native void resolve(MemberName self, Class<?> caller);
    static native int getMembers(Class<?> defc, String matchName, String matchSig,
            int matchFlags, Class<?> caller, int skip, MemberName[] results);

    /** Initialize a method type, once per form. */
    static void init(MethodType self) { }

    /** Fetch the name of the handled method, if available.
     *  This routine is for debugging and reflection.
     */
    static MemberName getMethodName(MethodHandle self) {
        throw new ikvm.internal.NotYetImplementedError();
    }

    static Object[] makeTarget(Class<?> defc, String name, String sig, int mods, Class<?> refc) {
        return new Object[] { defc, name, sig, mods, refc };
    }

    /** Java copy of MethodHandlePushLimit in range 2..255. */
    static final int JVM_PUSH_LIMIT;
    /** JVM stack motion (in words) after one slot is pushed, usually -1.
     */
    static final int JVM_STACK_MOVE_UNIT;

    /** Which conv-ops are implemented by the JVM? */
    static final int CONV_OP_IMPLEMENTED_MASK;
    /** Derived mode flag.  Only false on some old JVM implementations. */
    static final boolean HAVE_RICOCHET_FRAMES;

    static final int OP_ROT_ARGS_DOWN_LIMIT_BIAS;

    static {
        int k;
        JVM_PUSH_LIMIT              = 3;
        JVM_STACK_MOVE_UNIT         = 1;
        k                           = 0;
        CONV_OP_IMPLEMENTED_MASK    = (k != 0) ? k : DEFAULT_CONV_OP_IMPLEMENTED_MASK;
        k                           = 0;
        OP_ROT_ARGS_DOWN_LIMIT_BIAS = (k != 0) ? (byte)k : -1;
        HAVE_RICOCHET_FRAMES        = (CONV_OP_IMPLEMENTED_MASK & (1<<OP_COLLECT_ARGS)) != 0;
        //sun.reflect.Reflection.registerMethodsToFilter(MethodHandleImpl.class, "init");
    }

    // All compile-time constants go here.
    // There is an opportunity to check them against the JVM's idea of them.
    static class Constants {
        Constants() { } // static only
        // MethodHandleImpl
        static final int // for getConstant
                GC_JVM_PUSH_LIMIT = 0,
                GC_JVM_STACK_MOVE_UNIT = 1,
                GC_CONV_OP_IMPLEMENTED_MASK = 2,
                GC_OP_ROT_ARGS_DOWN_LIMIT_BIAS = 3;
        static final int
                ETF_HANDLE_OR_METHOD_NAME = 0, // all available data (immediate MH or method)
                ETF_DIRECT_HANDLE         = 1, // ultimate method handle (will be a DMH, may be self)
                ETF_METHOD_NAME           = 2, // ultimate method as MemberName
                ETF_REFLECT_METHOD        = 3; // ultimate method as java.lang.reflect object (sans refClass)

        // MemberName
        // The JVM uses values of -2 and above for vtable indexes.
        // Field values are simple positive offsets.
        // Ref: src/share/vm/oops/methodOop.hpp
        // This value is negative enough to avoid such numbers,
        // but not too negative.
        static final int
                MN_IS_METHOD           = 0x00010000, // method (not constructor)
                MN_IS_CONSTRUCTOR      = 0x00020000, // constructor
                MN_IS_FIELD            = 0x00040000, // field
                MN_IS_TYPE             = 0x00080000, // nested type
                MN_SEARCH_SUPERCLASSES = 0x00100000, // for MHN.getMembers
                MN_SEARCH_INTERFACES   = 0x00200000, // for MHN.getMembers
                VM_INDEX_UNINITIALIZED = -99;

        // BoundMethodHandle
        /** Constants for decoding the vmargslot field, which contains 2 values. */
        static final int
            ARG_SLOT_PUSH_SHIFT = 16,
            ARG_SLOT_MASK = (1<<ARG_SLOT_PUSH_SHIFT)-1;

        // AdapterMethodHandle
        /** Conversions recognized by the JVM.
         *  They must align with the constants in java.lang.invoke.AdapterMethodHandle,
         *  in the JVM file hotspot/src/share/vm/classfile/javaClasses.hpp.
         */
        static final int
            OP_RETYPE_ONLY   = 0x0, // no argument changes; straight retype
            OP_RETYPE_RAW    = 0x1, // straight retype, trusted (void->int, Object->T)
            OP_CHECK_CAST    = 0x2, // ref-to-ref conversion; requires a Class argument
            OP_PRIM_TO_PRIM  = 0x3, // converts from one primitive to another
            OP_REF_TO_PRIM   = 0x4, // unboxes a wrapper to produce a primitive
            OP_PRIM_TO_REF   = 0x5, // boxes a primitive into a wrapper
            OP_SWAP_ARGS     = 0x6, // swap arguments (vminfo is 2nd arg)
            OP_ROT_ARGS      = 0x7, // rotate arguments (vminfo is displaced arg)
            OP_DUP_ARGS      = 0x8, // duplicates one or more arguments (at TOS)
            OP_DROP_ARGS     = 0x9, // remove one or more argument slots
            OP_COLLECT_ARGS  = 0xA, // combine arguments using an auxiliary function
            OP_SPREAD_ARGS   = 0xB, // expand in place a varargs array (of known size)
            OP_FOLD_ARGS     = 0xC, // combine but do not remove arguments; prepend result
            //OP_UNUSED_13   = 0xD, // unused code, perhaps for reified argument lists
            CONV_OP_LIMIT    = 0xE; // limit of CONV_OP enumeration
        /** Shift and mask values for decoding the AMH.conversion field.
         *  These numbers are shared with the JVM for creating AMHs.
         */
        static final int
            CONV_OP_MASK     = 0xF00, // this nybble contains the conversion op field
            CONV_TYPE_MASK   = 0x0F,  // fits T_ADDRESS and below
            CONV_VMINFO_MASK = 0x0FF, // LSB is reserved for JVM use
            CONV_VMINFO_SHIFT     =  0, // position of bits in CONV_VMINFO_MASK
            CONV_OP_SHIFT         =  8, // position of bits in CONV_OP_MASK
            CONV_DEST_TYPE_SHIFT  = 12, // byte 2 has the adapter BasicType (if needed)
            CONV_SRC_TYPE_SHIFT   = 16, // byte 2 has the source BasicType (if needed)
            CONV_STACK_MOVE_SHIFT = 20, // high 12 bits give signed SP change
            CONV_STACK_MOVE_MASK  = (1 << (32 - CONV_STACK_MOVE_SHIFT)) - 1;

        /** Which conv-ops are implemented by the JVM? */
        static final int DEFAULT_CONV_OP_IMPLEMENTED_MASK =
                // Value to use if the corresponding JVM query fails.
                ((1<<OP_RETYPE_ONLY)
                |(1<<OP_RETYPE_RAW)
                |(1<<OP_CHECK_CAST)
                |(1<<OP_PRIM_TO_PRIM)
                |(1<<OP_REF_TO_PRIM)
                |(1<<OP_SWAP_ARGS)
                |(1<<OP_ROT_ARGS)
                |(1<<OP_DUP_ARGS)
                |(1<<OP_DROP_ARGS)
                //|(1<<OP_SPREAD_ARGS)
                );

        /**
         * Basic types as encoded in the JVM.  These code values are not
         * intended for use outside this class.  They are used as part of
         * a private interface between the JVM and this class.
         */
        static final int
            T_BOOLEAN  =  4,
            T_CHAR     =  5,
            T_FLOAT    =  6,
            T_DOUBLE   =  7,
            T_BYTE     =  8,
            T_SHORT    =  9,
            T_INT      = 10,
            T_LONG     = 11,
            T_OBJECT   = 12,
            //T_ARRAY    = 13
            T_VOID     = 14,
            //T_ADDRESS  = 15
            T_ILLEGAL  = 99;

        /**
         * Constant pool reference-kind codes, as used by CONSTANT_MethodHandle CP entries.
         */
        static final int
            REF_getField                = 1,
            REF_getStatic               = 2,
            REF_putField                = 3,
            REF_putStatic               = 4,
            REF_invokeVirtual           = 5,
            REF_invokeStatic            = 6,
            REF_invokeSpecial           = 7,
            REF_newInvokeSpecial        = 8,
            REF_invokeInterface         = 9;
    }

    /**
     * This assertion marks code which was written before ricochet frames were implemented.
     * Such code will go away when the ports catch up.
     */
    static boolean workaroundWithoutRicochetFrames() {
        assert(!HAVE_RICOCHET_FRAMES) : "this code should not be executed if `-XX:+UseRicochetFrames is enabled";
        return true;
    }
}
