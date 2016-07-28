#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QPainter>
#include <QTimer>

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

private:
    virtual void paintEvent(QPaintEvent *);
    void draw(QPainter *painter);
private slots:
    void RefreshView();

private:
    QTimer *m_timer;
    Ui::MainWindow *ui;

 private:
    int m_nPntPerScreen;
    int m_nIndex;
    float *m_pData;
};

#endif // MAINWINDOW_H
