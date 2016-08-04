#-------------------------------------------------
#
# Project created by QtCreator 2016-07-28T10:31:15
#
#-------------------------------------------------

QT       += core gui
QMAKE_CXXFLAGS += -std=c++11

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = FftView
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    kfft.cpp

HEADERS  += mainwindow.h \
    kfft.h

FORMS    += mainwindow.ui
