/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package tracelab.project.template;

import java.awt.HeadlessException;
import java.awt.Insets;
import java.awt.TextField;
import java.awt.event.ActionEvent;
import java.io.File;
import javax.swing.ImageIcon;
import javax.swing.JFileChooser;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.event.DocumentEvent;
import javax.swing.event.DocumentListener;
import javax.swing.text.Document;
import org.openide.filesystems.FileUtil;
import org.openide.util.NbBundle;

public final class TraceLabProjectConfigurationVisualPanel1 extends JPanel implements DocumentListener {

    public static final String PROP_TRACELAB_LIB_DIR = "tracelablib";
    private TraceLabProjectConfigurationWizardPanel1 panel;
    
    private static ImageIcon WARNING_ICON;
    private static ImageIcon CORRECT_ICON;
           
    private static String TraceLabSdkDll = "TraceLabSDK.dll";
    private static String TraceLabSdkJar = "TraceLabSDK.Jar";
    private static String mscorlibjar = "mscorlib.jar";
    
    private static String TraceLabSdkTypesDll = "TraceLabSDK.Types.dll";
    private static String TraceLabSdkTypesJar = "TraceLabSDK.Types.jar";
    
    
    /**
     * Creates new form TraceLabProjectConfigurationVisualPanel1
     */
    public TraceLabProjectConfigurationVisualPanel1(TraceLabProjectConfigurationWizardPanel1 panel) {
        initIcons();    
        initComponents();
        this.panel = panel;
        traceLabSDKLibDir.getDocument().addDocumentListener(this);
        tracelabSDKTypesDir.getDocument().addDocumentListener(this);
        
        String value = WindowsRegistry.readRegistry("HKEY_CURRENT_USER\\SOFTWARE\\COEST\\TraceLab", "Components");
        if(value != null && value.equals("") == false) {
            componentsDirField.setText(value);
        }
        
        TrySetFromRegistryInstallDir("Lib", traceLabSDKLibDir);
        
        TrySetFromRegistryInstallDir("Types", tracelabSDKTypesDir);
        
        WarnIfFileDoesNotExists(componentsDirField.getText(), "Components directory has not been found", warningComponentsDirMissing);
        WarnIfFileDoesNotExists(ikvmDir.getText()+File.separator+"ikvmc.exe", "IKVM directory with ikvmc.exe not found. (IKVM/bin folder needs to have ikvmc.exe application)", warningIKVMMissing);
        WarnIfFileDoesNotExists(traceLabSDKLibDir.getText(), "TraceLab Lib directory has not been found", warningLibDirMissing);
        WarnIfFileDoesNotExists(tracalabSdkDllLocation.getText(), TraceLabSdkDll + " has not been found in the given directory", warningTraceLabSDKDllMissing);
        WarnIfFileDoesNotExists(tracelabSDKJarLocation.getText(), TraceLabSdkJar + " has not been found in the given directory", warningTraceLabSdkJarMissing);
        WarnIfFileDoesNotExists(mscorlibJarLocation.getText(), mscorlibjar + " has not been found in the given directory", warningMscorlibMissing);
        WarnIfFileDoesNotExists(tracelabSDKTypesDir.getText(), "Types directory has not been found", warningTypesDirMissing);
        WarnIfFileDoesNotExists(tracelabTypesDllLocation.getText(), TraceLabSdkTypesDll + " has not been found in the given directory", warningTraceLabTypesDllMissing);
        WarnIfFileDoesNotExists(tracelabTypesJarLocation.getText(), TraceLabSdkTypesJar + " has not been found in the given directory", warningTraceLabTypesJarMissing);
        
    }
    
    private void initIcons() 
    {
        WARNING_ICON = new ImageIcon(getClass().getResource("/tracelab/project/template/resources/warning-icon.png"));
        CORRECT_ICON = new ImageIcon(getClass().getResource("/tracelab/project/template/resources/correct-icon.png"));
    }

    private void TrySetFromRegistryInstallDir(String key, JTextField field) {
        String value = WindowsRegistry.readRegistry("HKEY_LOCAL_MACHINE\\SOFTWARE\\COEST\\TraceLab", key);
        if(value != null && value.equals("") == false) {
            field.setText(value);
        } else {
            //try again in syswow64 for 64bit computers
            value = WindowsRegistry.readRegistry("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\COEST\\TraceLab", key);
            if(value != null && value.equals("") == false) {
                field.setText(value);
            }
        }
    }

    @Override
    public String getName() {
        return NbBundle.getMessage(TraceLabProjectConfigurationWizardPanel1.class, "LBL_CreateProjectStep1LongName");
    }

    private void DoBrowse(ActionEvent evt, String dialogTitle, JTextField textFieldToUpdate) throws HeadlessException {
        String command = evt.getActionCommand();
        if ("BROWSE".equals(command)) {
            JFileChooser chooser = new JFileChooser();
            FileUtil.preventFileChooserSymlinkTraversal(chooser, null);
            chooser.setDialogTitle(dialogTitle);
            chooser.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
            String path = textFieldToUpdate.getText();
            if (path.length() > 0) {
                File f = new File(path);
                if (f.exists()) {
                    chooser.setSelectedFile(f);
                }
            }
            if (JFileChooser.APPROVE_OPTION == chooser.showOpenDialog(this)) {
                File projectDir = chooser.getSelectedFile();
                textFieldToUpdate.setText(FileUtil.normalizeFile(projectDir).getAbsolutePath());
            }
        }
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();
        jLabel3 = new javax.swing.JLabel();
        tracalabSdkDllLocation = new javax.swing.JTextField();
        jLabel4 = new javax.swing.JLabel();
        componentsDirField = new javax.swing.JTextField();
        tracelabTypesDllLocation = new javax.swing.JTextField();
        ikvmDir = new javax.swing.JTextField();
        jLabel5 = new javax.swing.JLabel();
        tracelabSDKJarLocation = new javax.swing.JTextField();
        jLabel6 = new javax.swing.JLabel();
        tracelabTypesJarLocation = new javax.swing.JTextField();
        jLabel7 = new javax.swing.JLabel();
        mscorlibJarLocation = new javax.swing.JTextField();
        browseButton = new javax.swing.JButton();
        browseButtonIKVM = new javax.swing.JButton();
        jLabel8 = new javax.swing.JLabel();
        traceLabSDKLibDir = new javax.swing.JTextField();
        browseButtonTraceLabSDKLib = new javax.swing.JButton();
        jLabel9 = new javax.swing.JLabel();
        tracelabSDKTypesDir = new javax.swing.JTextField();
        browseButtonTraceLabSDKTypesDir = new javax.swing.JButton();
        warningIKVMMissing = new javax.swing.JLabel();
        warningLibDirMissing = new javax.swing.JLabel();
        warningComponentsDirMissing = new javax.swing.JLabel();
        warningTraceLabSDKDllMissing = new javax.swing.JLabel();
        warningTraceLabSdkJarMissing = new javax.swing.JLabel();
        warningMscorlibMissing = new javax.swing.JLabel();
        warningTypesDirMissing = new javax.swing.JLabel();
        warningTraceLabTypesDllMissing = new javax.swing.JLabel();
        warningTraceLabTypesJarMissing = new javax.swing.JLabel();
        jScrollPane1 = new javax.swing.JScrollPane();
        jTextArea1 = new javax.swing.JTextArea();

        org.openide.awt.Mnemonics.setLocalizedText(jLabel1, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel1.text")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(jLabel2, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel2.text")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(jLabel3, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel3.text")); // NOI18N
        jLabel3.setToolTipText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel3.toolTipText")); // NOI18N

        tracalabSdkDllLocation.setEditable(false);
        tracalabSdkDllLocation.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.tracalabSdkDllLocation.text")); // NOI18N
        tracalabSdkDllLocation.setToolTipText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.tracalabSdkDllLocation.toolTipText")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(jLabel4, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel4.text")); // NOI18N

        componentsDirField.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.componentsDirField.text")); // NOI18N
        componentsDirField.setToolTipText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.componentsDirField.toolTipText")); // NOI18N

        tracelabTypesDllLocation.setEditable(false);
        tracelabTypesDllLocation.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.tracelabTypesDllLocation.text")); // NOI18N

        ikvmDir.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.ikvmDir.text")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(jLabel5, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel5.text")); // NOI18N

        tracelabSDKJarLocation.setEditable(false);
        tracelabSDKJarLocation.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.tracelabSDKJarLocation.text")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(jLabel6, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel6.text")); // NOI18N

        tracelabTypesJarLocation.setEditable(false);
        tracelabTypesJarLocation.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.tracelabTypesJarLocation.text")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(jLabel7, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel7.text")); // NOI18N

        mscorlibJarLocation.setEditable(false);
        mscorlibJarLocation.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.mscorlibJarLocation.text")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(browseButton, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.browseButton.text")); // NOI18N
        browseButton.setActionCommand(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.browseButton.actionCommand")); // NOI18N
        browseButton.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                browseButtonActionPerformed(evt);
            }
        });

        org.openide.awt.Mnemonics.setLocalizedText(browseButtonIKVM, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.browseButtonIKVM.text")); // NOI18N
        browseButtonIKVM.setActionCommand(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.browseButtonIKVM.actionCommand")); // NOI18N
        browseButtonIKVM.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                browseButtonIKVMActionPerformed(evt);
            }
        });

        org.openide.awt.Mnemonics.setLocalizedText(jLabel8, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel8.text")); // NOI18N
        jLabel8.setToolTipText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel8.toolTipText")); // NOI18N

        traceLabSDKLibDir.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.traceLabSDKLibDir.text")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(browseButtonTraceLabSDKLib, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.browseButtonTraceLabSDKLib.text")); // NOI18N
        browseButtonTraceLabSDKLib.setActionCommand(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.browseButtonTraceLabSDKLib.actionCommand")); // NOI18N
        browseButtonTraceLabSDKLib.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                browseButtonTraceLabSDKLibActionPerformed(evt);
            }
        });

        org.openide.awt.Mnemonics.setLocalizedText(jLabel9, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jLabel9.text")); // NOI18N

        tracelabSDKTypesDir.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.tracelabSDKTypesDir.text")); // NOI18N

        org.openide.awt.Mnemonics.setLocalizedText(browseButtonTraceLabSDKTypesDir, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.browseButtonTraceLabSDKTypesDir.text")); // NOI18N
        browseButtonTraceLabSDKTypesDir.setActionCommand(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.browseButtonTraceLabSDKTypesDir.actionCommand")); // NOI18N
        browseButtonTraceLabSDKTypesDir.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                browseButtonTraceLabSDKTypesDirActionPerformed(evt);
            }
        });

        org.openide.awt.Mnemonics.setLocalizedText(warningIKVMMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningIKVMMissing.text")); // NOI18N
        warningIKVMMissing.setAlignmentY(0.0F);
        warningIKVMMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningIKVMMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningIKVMMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        org.openide.awt.Mnemonics.setLocalizedText(warningLibDirMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningLibDirMissing.text")); // NOI18N
        warningLibDirMissing.setAlignmentY(0.0F);
        warningLibDirMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningLibDirMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningLibDirMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        org.openide.awt.Mnemonics.setLocalizedText(warningComponentsDirMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningComponentsDirMissing.text")); // NOI18N
        warningComponentsDirMissing.setAlignmentY(0.0F);
        warningComponentsDirMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningComponentsDirMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningComponentsDirMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        org.openide.awt.Mnemonics.setLocalizedText(warningTraceLabSDKDllMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningTraceLabSDKDllMissing.text")); // NOI18N
        warningTraceLabSDKDllMissing.setAlignmentY(0.0F);
        warningTraceLabSDKDllMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningTraceLabSDKDllMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningTraceLabSDKDllMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        org.openide.awt.Mnemonics.setLocalizedText(warningTraceLabSdkJarMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningTraceLabSdkJarMissing.text")); // NOI18N
        warningTraceLabSdkJarMissing.setAlignmentY(0.0F);
        warningTraceLabSdkJarMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningTraceLabSdkJarMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningTraceLabSdkJarMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        org.openide.awt.Mnemonics.setLocalizedText(warningMscorlibMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningMscorlibMissing.text")); // NOI18N
        warningMscorlibMissing.setAlignmentY(0.0F);
        warningMscorlibMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningMscorlibMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningMscorlibMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        org.openide.awt.Mnemonics.setLocalizedText(warningTypesDirMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningTypesDirMissing.text")); // NOI18N
        warningTypesDirMissing.setAlignmentY(0.0F);
        warningTypesDirMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningTypesDirMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningTypesDirMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        org.openide.awt.Mnemonics.setLocalizedText(warningTraceLabTypesDllMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningTraceLabTypesDllMissing.text")); // NOI18N
        warningTraceLabTypesDllMissing.setAlignmentY(0.0F);
        warningTraceLabTypesDllMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningTraceLabTypesDllMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningTraceLabTypesDllMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        org.openide.awt.Mnemonics.setLocalizedText(warningTraceLabTypesJarMissing, org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.warningTraceLabTypesJarMissing.text")); // NOI18N
        warningTraceLabTypesJarMissing.setAlignmentY(0.0F);
        warningTraceLabTypesJarMissing.setMaximumSize(new java.awt.Dimension(16, 16));
        warningTraceLabTypesJarMissing.setMinimumSize(new java.awt.Dimension(16, 16));
        warningTraceLabTypesJarMissing.setPreferredSize(new java.awt.Dimension(16, 16));

        jTextArea1.setBackground(new java.awt.Color(242, 242, 242));
        jTextArea1.setColumns(20);
        jTextArea1.setEditable(false);
        jTextArea1.setFont(new java.awt.Font("Arial", 0, 12)); // NOI18N
        jTextArea1.setLineWrap(true);
        jTextArea1.setRows(5);
        jTextArea1.setText(org.openide.util.NbBundle.getMessage(TraceLabProjectConfigurationVisualPanel1.class, "TraceLabProjectConfigurationVisualPanel1.jTextArea1.text")); // NOI18N
        jTextArea1.setWrapStyleWord(true);
        jTextArea1.setFocusable(false);
        jTextArea1.setHighlighter(null);
        jScrollPane1.setViewportView(jTextArea1);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(this);
        this.setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jLabel1)
                            .addComponent(jLabel2)
                            .addComponent(jLabel3)
                            .addComponent(jLabel5)
                            .addComponent(jLabel4)
                            .addComponent(jLabel6)
                            .addComponent(jLabel7)
                            .addComponent(jLabel8)
                            .addComponent(jLabel9))
                        .addGap(25, 25, 25)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addGroup(layout.createSequentialGroup()
                                .addComponent(tracelabSDKTypesDir, javax.swing.GroupLayout.PREFERRED_SIZE, 357, javax.swing.GroupLayout.PREFERRED_SIZE)
                                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                .addComponent(warningTypesDirMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                .addComponent(browseButtonTraceLabSDKTypesDir))
                            .addGroup(layout.createSequentialGroup()
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                                    .addComponent(tracelabSDKJarLocation, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, 438, Short.MAX_VALUE)
                                    .addComponent(tracalabSdkDllLocation, javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(mscorlibJarLocation))
                                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(warningTraceLabSdkJarMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                    .addComponent(warningMscorlibMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)))
                            .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                                .addComponent(warningTraceLabSDKDllMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                .addGroup(layout.createSequentialGroup()
                                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                                        .addComponent(ikvmDir, javax.swing.GroupLayout.Alignment.LEADING)
                                        .addComponent(componentsDirField, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, 357, Short.MAX_VALUE)
                                        .addComponent(traceLabSDKLibDir))
                                    .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                        .addGroup(layout.createSequentialGroup()
                                            .addComponent(warningComponentsDirMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                            .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                            .addComponent(browseButton))
                                        .addGroup(layout.createSequentialGroup()
                                            .addComponent(warningIKVMMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                            .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                            .addComponent(browseButtonIKVM))
                                        .addGroup(layout.createSequentialGroup()
                                            .addComponent(warningLibDirMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                            .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                            .addComponent(browseButtonTraceLabSDKLib)))))
                            .addGroup(layout.createSequentialGroup()
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                                    .addComponent(tracelabTypesJarLocation, javax.swing.GroupLayout.DEFAULT_SIZE, 436, Short.MAX_VALUE)
                                    .addComponent(tracelabTypesDllLocation))
                                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(warningTraceLabTypesDllMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                                    .addComponent(warningTraceLabTypesJarMissing, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)))))
                    .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 638, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel4)
                        .addComponent(componentsDirField, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(warningComponentsDirMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(browseButton))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(browseButtonIKVM)
                    .addComponent(warningIKVMMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel3)
                        .addComponent(ikvmDir, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel8)
                        .addComponent(traceLabSDKLibDir, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(warningLibDirMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(browseButtonTraceLabSDKLib))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel1)
                        .addComponent(tracalabSdkDllLocation, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(warningTraceLabSDKDllMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel5)
                        .addComponent(tracelabSDKJarLocation, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(warningTraceLabSdkJarMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(warningMscorlibMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel7)
                        .addComponent(mscorlibJarLocation, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)))
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel9)
                        .addComponent(tracelabSDKTypesDir, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(warningTypesDirMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(browseButtonTraceLabSDKTypesDir))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel2)
                        .addComponent(tracelabTypesDllLocation, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(warningTraceLabTypesDllMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                        .addComponent(jLabel6)
                        .addComponent(tracelabTypesJarLocation, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(warningTraceLabTypesJarMissing, javax.swing.GroupLayout.PREFERRED_SIZE, 16, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );
    }// </editor-fold>//GEN-END:initComponents

    private void browseButtonActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_browseButtonActionPerformed
        DoBrowse(evt, "Select components directory", componentsDirField);
        WarnIfFileDoesNotExists(componentsDirField.getText(), "Components directory has not been found", warningComponentsDirMissing);
    }//GEN-LAST:event_browseButtonActionPerformed

    private void browseButtonIKVMActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_browseButtonIKVMActionPerformed
        DoBrowse(evt, "Select IKVM bin directory", ikvmDir);
        WarnIfFileDoesNotExists(ikvmDir.getText()+File.separator+"ikvmc.exe", "IKVM directory with ikvmc.exe not found. (IKVM/bin folder needs to have ikvmc.exe application)", warningIKVMMissing);
    }//GEN-LAST:event_browseButtonIKVMActionPerformed

    private void browseButtonTraceLabSDKLibActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_browseButtonTraceLabSDKLibActionPerformed
        DoBrowse(evt, "Select TraceLabSDK lib directory", traceLabSDKLibDir);
        WarnIfFileDoesNotExists(traceLabSDKLibDir.getText(), "TraceLab Lib directory has not been found", warningLibDirMissing);
        WarnIfFileDoesNotExists(tracalabSdkDllLocation.getText(), TraceLabSdkDll + " has not been found in the given directory", warningTraceLabSDKDllMissing);
        WarnIfFileDoesNotExists(tracelabSDKJarLocation.getText(), TraceLabSdkJar + " has not been found in the given directory", warningTraceLabSdkJarMissing);
        WarnIfFileDoesNotExists(mscorlibJarLocation.getText(), mscorlibjar + " has not been found in the given directory", warningMscorlibMissing);
    }//GEN-LAST:event_browseButtonTraceLabSDKLibActionPerformed

    private void browseButtonTraceLabSDKTypesDirActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_browseButtonTraceLabSDKTypesDirActionPerformed
        DoBrowse(evt, "Select TraceLabSDK Types directory", tracelabSDKTypesDir);
        WarnIfFileDoesNotExists(tracelabSDKTypesDir.getText(), "Types directory has not been found", warningTypesDirMissing);
        WarnIfFileDoesNotExists(tracelabTypesDllLocation.getText(), TraceLabSdkTypesDll + " has not been found in the given directory", warningTraceLabTypesDllMissing);
        WarnIfFileDoesNotExists(tracelabTypesJarLocation.getText(), TraceLabSdkTypesJar + " has not been found in the given directory", warningTraceLabTypesJarMissing);
    }//GEN-LAST:event_browseButtonTraceLabSDKTypesDirActionPerformed

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton browseButton;
    private javax.swing.JButton browseButtonIKVM;
    private javax.swing.JButton browseButtonTraceLabSDKLib;
    private javax.swing.JButton browseButtonTraceLabSDKTypesDir;
    private javax.swing.JTextField componentsDirField;
    private javax.swing.JTextField ikvmDir;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JLabel jLabel5;
    private javax.swing.JLabel jLabel6;
    private javax.swing.JLabel jLabel7;
    private javax.swing.JLabel jLabel8;
    private javax.swing.JLabel jLabel9;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JTextArea jTextArea1;
    private javax.swing.JTextField mscorlibJarLocation;
    private javax.swing.JTextField tracalabSdkDllLocation;
    private javax.swing.JTextField traceLabSDKLibDir;
    private javax.swing.JTextField tracelabSDKJarLocation;
    private javax.swing.JTextField tracelabSDKTypesDir;
    private javax.swing.JTextField tracelabTypesDllLocation;
    private javax.swing.JTextField tracelabTypesJarLocation;
    private javax.swing.JLabel warningComponentsDirMissing;
    private javax.swing.JLabel warningIKVMMissing;
    private javax.swing.JLabel warningLibDirMissing;
    private javax.swing.JLabel warningMscorlibMissing;
    private javax.swing.JLabel warningTraceLabSDKDllMissing;
    private javax.swing.JLabel warningTraceLabSdkJarMissing;
    private javax.swing.JLabel warningTraceLabTypesDllMissing;
    private javax.swing.JLabel warningTraceLabTypesJarMissing;
    private javax.swing.JLabel warningTypesDirMissing;
    // End of variables declaration//GEN-END:variables

    public String getComponentsDir() {
        return componentsDirField.getText();
    }

    public String getIkvmDir() {
        return ikvmDir.getText();
    }

    public String getTraceLabSdkDll() {
        return tracalabSdkDllLocation.getText();
    }

    public String getTraceLabTypesDll() {
        return tracelabTypesDllLocation.getText();
    }
    
    public String getTraceLabSdkJar() {
        return tracelabSDKJarLocation.getText();
    }

    public String getTraceLabTypesJar() {
        return tracelabTypesJarLocation.getText();
    }
    
    public String getMscorlibJar() {
        return mscorlibJarLocation.getText();
    }

    // Implementation of DocumentListener --------------------------------------
    public void changedUpdate(DocumentEvent e) {
        updateFields(e);
        if (this.traceLabSDKLibDir.getDocument() == e.getDocument()) {
            firePropertyChange(PROP_TRACELAB_LIB_DIR, null, traceLabSDKLibDir.getText());
        }
    }

    public void insertUpdate(DocumentEvent e) {
        updateFields(e);
        if (this.traceLabSDKLibDir.getDocument() == e.getDocument()) {
            firePropertyChange(PROP_TRACELAB_LIB_DIR, null, traceLabSDKLibDir.getText());
        }
    }

    public void removeUpdate(DocumentEvent e) {
        updateFields(e);
        if (this.traceLabSDKLibDir.getDocument() == e.getDocument()) {
            firePropertyChange(PROP_TRACELAB_LIB_DIR, null, traceLabSDKLibDir.getText());
        }
    }

    /**
     * Handles changes in the Project name and project directory,
     */
    private void updateFields(DocumentEvent e) {

        Document doc = e.getDocument();

        if (doc == traceLabSDKLibDir.getDocument()) {
            // Change in the project name
            String libFolder = traceLabSDKLibDir.getText();
            if(libFolder.endsWith(File.separator) == false) 
                libFolder = libFolder + File.separator;
            
            tracalabSdkDllLocation.setText(libFolder + TraceLabSdkDll);
            tracelabSDKJarLocation.setText(libFolder + TraceLabSdkJar);
            mscorlibJarLocation.setText(libFolder + mscorlibjar);
        }
        
        if (doc == tracelabSDKTypesDir.getDocument()) {
            // Change in the project name
            String typesFolder = tracelabSDKTypesDir.getText();
            
            if(typesFolder.endsWith(File.separator) == false) 
                typesFolder = typesFolder + File.separator;
            
            tracelabTypesDllLocation.setText(typesFolder + TraceLabSdkTypesDll);
            tracelabTypesJarLocation.setText(typesFolder + TraceLabSdkTypesJar);
        }
    }

    private static void WarnIfFileDoesNotExists(String filePath, String tooltipIfNotFound, javax.swing.JLabel labelIcon) 
    {
        File f = new File(filePath);
        if(f.exists()) 
        { 
            labelIcon.setIcon(CORRECT_ICON);
        } else {
            labelIcon.setIcon(WARNING_ICON);
            labelIcon.setToolTipText(tooltipIfNotFound);
            labelIcon.setIconTextGap(0);
            labelIcon.setBorder(null);
            labelIcon.setText(null);
            labelIcon.setSize(WARNING_ICON.getImage().getWidth(null), WARNING_ICON.getImage().getHeight(null));
        }
    }
    
}
