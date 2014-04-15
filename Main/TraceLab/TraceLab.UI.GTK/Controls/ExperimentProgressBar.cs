// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.
//
using System;

namespace TraceLab.UI.GTK
{
    /// <summary>
    /// Experiment progress bar control shows current status and progress of experiment. 
    /// 
    /// Note, that ExperimentProgressBar is used by the experiment thread, 
    /// so to avoid any threading problems with GTK,
    /// setting methods are executed using the Gtk.Application.Invoke() method
    /// to ensure their execution on the GTK+ main loop thread.
    /// </summary>
    public class ExperimentProgressBar : Gtk.ProgressBar, TraceLabSDK.IProgress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceLab.UI.GTK.ExperimentProgressBar"/> class.
        /// </summary>
        public ExperimentProgressBar() : base()
        {
        }

        private bool m_isIndeterminate = true;
        /// <summary>
        /// Gets or sets a value indicating whether the progress of whatever is processing is indeterminate.
        /// 
        /// Eg. The number of steps cannot be determined, or the current step cannot be determined.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is indeterminate; otherwise, <c>false</c>.
        /// </value>
        public bool IsIndeterminate
        {
            get
            {
                return m_isIndeterminate;
            }
            set
            {
                //assure that value is set using GTK+ main loop thread to avoid any threading problems
                Gtk.Application.Invoke(delegate 
                {
                    m_isIndeterminate = value;
                });
            }
        }

        /// <summary>
        /// Gets or sets the number of steps that must be completed.
        /// 
        /// In this case it wraps parent Adjustment property.
        /// </summary>
        /// <value>
        /// The num steps.
        /// </value>
        public double NumSteps
        {
            get
            {
                return Adjustment.Upper;
            }
            set
            {
                //assure that value is set using GTK+ main loop thread to avoid any threading problems
                Gtk.Application.Invoke(delegate 
                {
                    Adjustment = new Gtk.Adjustment(0.0, 0.0, value, 1.0, 1.0, 1.0);
                });
            }
        }
        
        private double m_currentStep;
        /// <summary>
        /// Gets or sets the current step.
        /// It doesn't matter in current implementation of ProgressBar, but must be implemented
        /// because of IProgress interface. 
        /// </summary>
        /// <value>
        /// The current step.
        /// </value>
        public double CurrentStep
        {
            get
            {
                return m_currentStep;
            }
            set
            {
                //assure that value is set using GTK+ main loop thread to avoid any threading problems
                Gtk.Application.Invoke(delegate 
                {
                    m_currentStep = value;
                });
            }
        }

        /// <summary>
        /// Gets or sets the current status.
        /// 
        /// It wraps parent class Text property
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        /// <remarks>
        /// This is usually a description of what the current step is doing.
        /// </remarks>
        public string CurrentStatus
        {
            get
            {
                return Text;
            }
            set
            {
                //assure that value is set using GTK+ main loop thread to avoid any threading problems
                Gtk.Application.Invoke(delegate 
                {
                    if (Text != value)
                    {
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            Text = String.Format("Current Status: {0}", value);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Resets the current progress.
        /// </summary>
        public void Reset()
        {
            //assure that value is set using GTK+ main loop thread to avoid any threading problems
            Gtk.Application.Invoke(delegate 
            {
                CurrentStep = 0;
                IsIndeterminate = false;
                CurrentStatus = string.Empty;
                Adjustment = new Gtk.Adjustment(0.0, 0.0, 0.0, 1.0, 1.0, 1.0);
                //Fraction = 0; //setting fraction to 0 resets the bar after pulsing in inteterminate state
            });
        }

        /// <summary>
        /// Increments the current progress to the next step.
        /// </summary>
        public void Increment()
        {
            //assure that value is set using GTK+ main loop thread to avoid any threading problems
            Gtk.Application.Invoke(delegate 
            {
                //if progress bar is set to indeterminate just pulse, otherwise increase Adjustment value
                if(m_isIndeterminate) 
                {
                    //TODO: if pulse is initially done, then even though progress is reset it still pulses
                    //something with reset is not right. it has low priority, so it should be investigated later
                    //Pulse();
                } 
                else 
                {
                    Adjustment.Value += 1;
                }
            });
        }
        
        private bool m_hasError;

        /// <summary>
        /// Whether the Progress implementation should change it's display to display an error
        /// </summary>
        /// <param name='hasError'>
        /// If set to <c>true</c> has error.
        /// </param>
        public void SetError(bool hasError)
        {
            //assure that value is set using GTK+ main loop thread to avoid any threading problems
            Gtk.Application.Invoke(delegate 
            {
                if(hasError == true) 
                {
                    Gdk.Color col = new Gdk.Color(255, 0, 0);
                    ModifyFg(Gtk.StateType.Normal, col);
                } 
                else 
                {
                    Gdk.Color col = new Gdk.Color(0, 0, 0);
                    ModifyFg(Gtk.StateType.Normal, col);
                }
                m_hasError = hasError;
            });
        }

        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError
        {
            get
            {
                return m_hasError;
            }
        }
    }
}

