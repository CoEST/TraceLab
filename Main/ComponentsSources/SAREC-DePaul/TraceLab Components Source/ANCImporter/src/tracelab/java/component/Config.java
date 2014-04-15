/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package tracelab.java.component;

import cli.TraceLabSDK.Component.Config.FilePath;

/**
 *
 * @author mgibiec
 */
public class Config
{

    private FilePath pathToAnc;

    public void setPathToAnc(FilePath pathToAnc) {
        this.pathToAnc = pathToAnc;
    }

    public FilePath getPathToAnc() {
        return pathToAnc;
    }

}
