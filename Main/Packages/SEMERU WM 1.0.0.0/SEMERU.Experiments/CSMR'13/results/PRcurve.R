#!/usr/bin/RScript

# 1 - title
# 2 - path
# 3 - output path
args <- commandArgs(TRUE)

results.VSM <- read.table(paste(args[2], "/VSM", sep=""), header=FALSE)
results.VSM_OCSTI <- read.table(paste(args[2], "/VSM_OCSTI", sep=""), header=FALSE)
results.VSM_UDCSTI <- read.table(paste(args[2], "/VSM_UDCSTI", sep=""), header=FALSE)
results.JS <- read.table(paste(args[2], "/JS", sep=""), header=FALSE)
results.JS_OCSTI <- read.table(paste(args[2], "/JS_OCSTI", sep=""), header=FALSE)
results.JS_UDCSTI <- read.table(paste(args[2], "/JS_UDCSTI", sep=""), header=FALSE)

pdf(paste(args[3], "/", args[1], ".pdf", sep=""), paper="USr")
par(mfrow=c(2,2))
plot(results.VSM, type="l", lty="solid", xlab="Recall", ylab="Precision", ylim=c(0,1), xlim=c(10,100), main=paste(args[1], "\nVSM"), lwd=2)
lines(results.VSM_OCSTI, lty="dotted", col="blue", lwd=2)
lines(results.VSM_UDCSTI, lty="dashed", col="green", lwd=2)
legend("topright", NULL, c("IR", "IR + O-CSTI", "IR + UD-CSTI"), lty=c("solid", "dotted", "dashed"), col=c("black", "blue", "green"), lwd=2)
plot(results.JS, type="l", lty="solid", xlab="Recall", ylab="Precision", ylim=c(0,1), xlim=c(10,100), main=paste(args[1], "\nJS"), lwd=2)
lines(results.JS_OCSTI, lty="dotted", col="blue", lwd=2)
lines(results.JS_UDCSTI, lty="dashed", col="green", lwd=2)
legend("topright", NULL, c("IR", "IR + O-CSTI", "IR + UD-CSTI"), lty=c("solid", "dotted", "dashed"), col=c("black", "blue", "green"), lwd=2)
silence <- dev.off()