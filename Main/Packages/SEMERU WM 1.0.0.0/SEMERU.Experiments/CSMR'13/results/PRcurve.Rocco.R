#!/usr/bin/RScript

# 1 - title
# 2 - path
# 3 - output path
args <- commandArgs(TRUE)

results.MyVSM <- read.table(paste(args[2], "/EasyClinic UC-CC.VSM.MYmetrics", sep=""), header=FALSE)
results.MyVSM_UDCSTI <- read.table(paste(args[2], "/EasyClinic UC-CC.VSM_UDCSTI.MYmetrics", sep=""), header=FALSE)
results.VSM <- read.table(paste(args[2], "/EasyClinic UC-CC.VSM.metrics", sep=""), header=FALSE)
results.VSM_UDCSTI <- read.table(paste(args[2], "/EasyClinic UC-CC.VSM_UDCSTI.metrics", sep=""), header=FALSE)
results.VSM_MyComputedUDCSTI <- read.table(paste(args[2], "/EasyClinic UC-CC.VSM_UDCSTI.MYsims.metrics", sep=""), header=FALSE)

pdf(paste(args[3], "/", args[1], ".pdf", sep=""), paper="USr")
par(mfrow=c(2,2))
plot(results.MyVSM, type="l", lty="solid", xlab="Recall", ylab="Precision", ylim=c(0,1), xlim=c(10,100), main="Rocco EasyClinic UC-CC VSM MyMetrics", lwd=2)
lines(results.MyVSM_UDCSTI, lty="solid", col="green", lwd=2)
legend("topright", NULL, c("IR", "IR + UD-CSTI"), lty=c("solid", "solid"), col=c("black", "green"), lwd=2)
plot(results.VSM, type="l", lty="solid", xlab="Recall", ylab="Precision", ylim=c(0,1), xlim=c(0.1,1), main="Rocco EasyClinic UC-CC VSM RoccoMetrics", lwd=2)
lines(results.VSM_UDCSTI, lty="solid", col="green", lwd=2)
legend("topright", NULL, c("IR", "IR + UD-CSTI"), lty=c("solid", "solid"), col=c("black", "green"), lwd=2)
plot(results.MyVSM, type="l", lty="solid", xlab="Recall", ylab="Precision", ylim=c(0,1), xlim=c(10,100), main="Rocco EasyClinic UC-CC VSM MyUD-CSTI", lwd=2)
lines(results.VSM_MyComputedUDCSTI, lty="solid", col="green", lwd=2)
legend("topright", NULL, c("IR", "IR + UD-CSTI"), lty=c("solid", "solid"), col=c("black", "green"), lwd=2)
silence <- dev.off()