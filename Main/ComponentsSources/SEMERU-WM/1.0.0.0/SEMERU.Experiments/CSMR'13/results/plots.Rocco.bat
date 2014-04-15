@echo off
echo Creating plots...
set savedir=%CD%
set basepath=C:/Users/Evan/Documents/SEMERU/TraceLab/trunk/SEMERU.Experiments/CSMR'13/results/
set script=%basepath%/PRcurve.Rocco.R
cd C:/Program Files/R/R-2.13.1/bin
echo Plotting Rocco results...
Rscript %script% Rocco %basepath%/CSMRTools/RoccoResults %basepath%/CSMRTools/RoccoResults
cd %savedir%
echo Done.