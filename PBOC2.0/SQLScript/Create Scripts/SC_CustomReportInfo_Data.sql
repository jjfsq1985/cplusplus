insert into SC_CustomReportInfo values(1,'FID','编号',50,NULL,NULL,0);
insert into SC_CustomReportInfo values(1,'FUserCardNo','用户卡号',100,'cstCount','#,###',0);
insert into SC_CustomReportInfo values(1,'Plate','车牌号',60,NULL,NULL,10);
insert into SC_CustomReportInfo values(1,'FTradeDateTime','消费时间',160,NULL,NULL,0);
insert into SC_CustomReportInfo values(1,'FPrice','价格',60,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(1,'FGas','气量',80,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(1,'FMoney','金额',80,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(1,'FResidualAmount','余额',120,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(1,'FGunNo','枪号',50,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(1,'ClientName','单位名称',150,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(1,'FOperatorCardNo','员工卡号',150,NULL,NULL,NULL);
GO

insert into SC_CustomReportInfo values(2,'FGas','总气量',150,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(2,'FMoney','总金额',150,'cstSum','#,##0.00',20);
insert into SC_CustomReportInfo values(2,'FNum','交易次数',100,'cstSum','#,###',20);
insert into SC_CustomReportInfo values(2,'FGunNo','枪号',50,NULL,NULL,20);
insert into SC_CustomReportInfo values(2,'FOperatorCardNo','操作员卡号',150,NULL,NULL,30);
insert into SC_CustomReportInfo values(2,'ClientName','所属公司',150,NULL,NULL,50);
GO

insert into SC_CustomReportInfo values(3,'StationNo','站点编号',100,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'GunNo','灰卡枪号',50,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'TradeDateTime','灰卡时间',150,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'GrayPrice','灰交易单价',100,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'GrayGas','灰交易气量',100,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'GrayMoney','灰交易金额',150,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'ResidualAmount','卡上余额',150,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'Operator','解灰操作员',150,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'OperateTime','解灰时间',150,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(3,'CardNo','灰卡卡号',150,NULL,NULL,NULL);
GO

insert into SC_CustomReportInfo values(4,'CardNum','卡号',150,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(4,'RechargeDateTime','充值时间',160,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(4,'ForwardBalance','充值前余额',120,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(4,'RechargeValue','充值金额',100,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(4,'PreferentialVal','优惠金额',100,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(4,'ReceivedVal','实际充值金额',100,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(4,'CurrentBalance','充值后金额',100,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(4,'OperatorId','操作员ID',80,NULL,NULL,NULL);
GO

insert into SC_CustomReportInfo values(5,'CardNum','卡号',150,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(5,'Plate','车牌号',80,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(5,'FTradeDateTime','最后交易时间',160,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(5,'RechargeDateTime','最后充值时间',160,NULL,NULL,NULL);
insert into SC_CustomReportInfo values(5,'FGas','总加气量',120,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(5,'FMoney','总加气金额',150,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(5,'RechargeValue','总充值金额',150,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(5,'AccountBalance','当前卡余额',150,'cstSum','#,##0.00',NULL);
insert into SC_CustomReportInfo values(5,'AC','余额差',100,'cstSum','#,##0.00',NULL);
GO 