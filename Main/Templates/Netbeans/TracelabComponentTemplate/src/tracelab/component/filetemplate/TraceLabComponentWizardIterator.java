package tracelab.component.filetemplate;

import java.awt.Component;
import java.io.File;
import java.io.IOException;
import java.text.MessageFormat;
import java.util.*;
import javax.swing.JComponent;
import javax.swing.event.ChangeListener;
import org.netbeans.api.java.project.JavaProjectConstants;
import org.netbeans.api.project.Project;
import org.netbeans.api.project.ProjectManager;
import org.netbeans.api.project.ProjectUtils;
import org.netbeans.api.project.SourceGroup;
import org.netbeans.api.project.Sources;
import org.netbeans.api.templates.TemplateRegistration;
import org.netbeans.api.templates.TemplateRegistrations;
import org.netbeans.spi.java.project.support.ui.templates.JavaTemplates;
import org.netbeans.spi.project.ui.support.ProjectChooser;
import org.netbeans.spi.project.ui.templates.support.Templates;
import org.openide.WizardDescriptor;
import org.openide.filesystems.FileObject;
import org.openide.filesystems.FileUtil;
import org.openide.loaders.DataFolder;
import org.openide.loaders.DataObject;
import org.openide.util.Exceptions;
import org.openide.util.NbBundle;
import org.openide.util.NbBundle.Messages;

@TemplateRegistration(folder = "TraceLab", 
                      displayName = "TraceLab Component", 
                      description = "TraceLabComponentDescription.html", 
                      iconBase = "tracelab/component/filetemplate/resources/TraceLabIcon.png", 
                      content = "resources/component.java.template", 
                      position=100,
                      scriptEngine="freemarker")
public class TraceLabComponentWizardIterator implements WizardDescriptor.AsynchronousInstantiatingIterator<WizardDescriptor> {

    private int index;
    private WizardDescriptor.Panel<WizardDescriptor>[] panels;
    private WizardDescriptor wiz;

    public TraceLabComponentWizardIterator() {
    }

    public static TraceLabComponentWizardIterator createIterator() {
        return new TraceLabComponentWizardIterator();
    }

    private WizardDescriptor.Panel<WizardDescriptor>[] createPanels(WizardDescriptor wizardDescriptor) 
    {
        // Ask for Java folders
        Project project = Templates.getProject( wizardDescriptor );
        if (project == null) throw new NullPointerException ("No project found for: " + wizardDescriptor);
        Sources sources = ProjectUtils.getSources(project);
        SourceGroup[] groups = sources.getSourceGroups(JavaProjectConstants.SOURCES_TYPE_JAVA);
        assert groups != null : "Cannot return null from Sources.getSourceGroups: " + sources;
        groups = checkNotNull (groups, sources);
        if (groups.length == 0) {
            groups = sources.getSourceGroups( Sources.TYPE_GENERIC ); 
            groups = checkNotNull (groups, sources);
            return new WizardDescriptor.Panel[] {            
                Templates.buildSimpleTargetChooser(project, groups).create(),
                new ComponentDefinitionWizardPanel1()
            };
        } else {
            return new WizardDescriptor.Panel[]{
                JavaTemplates.createPackageChooser( project, groups ),
                new ComponentDefinitionWizardPanel1()
            };
        }
    }
    
    private static SourceGroup[] checkNotNull (SourceGroup[] groups, Sources sources) {
        List<SourceGroup> sourceGroups = new ArrayList<SourceGroup> ();
        for (SourceGroup sourceGroup : groups)
            if (sourceGroup == null)
                Exceptions.printStackTrace (new NullPointerException (sources + " returns null SourceGroup!"));
            else
                sourceGroups.add (sourceGroup);
        return sourceGroups.toArray (new SourceGroup [sourceGroups.size ()]);
    }

    private String[] createSteps(String[] before, WizardDescriptor.Panel[] panels) {
        assert panels != null;
        // hack to use the steps set before this panel processed
        int diff = 0;
        if (before == null) {
            before = new String[0];
        } else if (before.length > 0) {
            diff = ("...".equals (before[before.length - 1])) ? 1 : 0; // NOI18N
        }
        String[] res = new String[ (before.length - diff) + panels.length];
        for (int i = 0; i < res.length; i++) {
            if (i < (before.length - diff)) {
                res[i] = before[i];
            } else {
                res[i] = panels[i - before.length + diff].getComponent ().getName ();
            }
        }
        return res;
    }

    @Override
    public Set<FileObject> instantiate() throws IOException {
        
        FileObject dir = Templates.getTargetFolder( wiz );
        String targetName = Templates.getTargetName( wiz );
        
        DataFolder df = DataFolder.findFolder( dir );
        FileObject template = Templates.getTemplate( wiz );
        
        FileObject createdFile = null;
        
        DataObject dTemplate = DataObject.find( template );
        
        //get properties from the WizardDescriptor
        Map args = new HashMap();
        args.put("label", wiz.getProperty("component.label"));
        //parse new line of descriptions to \n+" 
        args.put("description", ((String)wiz.getProperty("component.description")).replace("\n", "\"\n+\""));
        args.put("includeSampleIO", wiz.getProperty("includeSampleIO"));
        
        DataObject dobj = dTemplate.createFromTemplate( df, targetName, args );
        createdFile = dobj.getPrimaryFile();

        return Collections.singleton( createdFile );
    }

    @Override
    public void initialize(WizardDescriptor wiz) {
        this.wiz = wiz;
        index = 0;
        panels = createPanels( wiz );
        // Make sure list of steps is accurate.
        String[] beforeSteps = null;
        Object prop = wiz.getProperty(WizardDescriptor.PROP_CONTENT_DATA);
        if (prop != null && prop instanceof String[]) {
            beforeSteps = (String[])prop;
        }
        String[] steps = createSteps (beforeSteps, panels);
        for (int i = 0; i < panels.length; i++) {
            Component c = panels[i].getComponent();
            if (steps[i] == null) {
                // Default step name to component name of panel.
                // Mainly useful for getting the name of the target
                // chooser to appear in the list of steps.
                steps[i] = c.getName();
            }
            if (c instanceof JComponent) { // assume Swing components
                JComponent jc = (JComponent)c;
                // Step #.
                jc.putClientProperty(WizardDescriptor.PROP_CONTENT_SELECTED_INDEX, new Integer(i));
                // Step name (actually the whole list for reference).
                jc.putClientProperty(WizardDescriptor.PROP_CONTENT_DATA, steps);
            }
        }
    }

    @Override
    public void uninitialize(WizardDescriptor wiz) {
        this.wiz = null;
        panels = null;
    }

    @Override
    public String name() {
        return MessageFormat.format("{0} of {1}",
                new Object[]{new Integer(index + 1), new Integer(panels.length)});
    }

    @Override
    public boolean hasNext() {
        return index < panels.length - 1;
    }

    @Override
    public boolean hasPrevious() {
        return index > 0;
    }

    @Override
    public void nextPanel() {
        if (!hasNext()) {
            throw new NoSuchElementException();
        }
        index++;
    }

    @Override
    public void previousPanel() {
        if (!hasPrevious()) {
            throw new NoSuchElementException();
        }
        index--;
    }

    @Override
    public WizardDescriptor.Panel<WizardDescriptor> current() {
        return panels[index];
    }

    // If nothing unusual changes in the middle of the wizard, simply:
    @Override
    public final void addChangeListener(ChangeListener l) {
    }

    @Override
    public final void removeChangeListener(ChangeListener l) {
    }
}
