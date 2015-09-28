insert into SC_Config values('S001','0',N'基本信息配置',N'',0,0.00,N'','Y','S',1032);
insert into SC_Config values('S002','0',N'硬件配置信息',N'',0,0.00,N'','Y','S',2305);
insert into SC_Config values('S003','0',N'软件配置信息',N'',0,0.00,N'','Y','S',2308);
insert into SC_Config values('S004','0',N'操作员',N'admin',0,0.00,N'','Y','S',2068);
insert into SC_Config values('S005','0',N'操作时间',N'2015-08-01 08:00:00',0,0.00,N'','Y','S',2069);
GO

insert into SC_Config values('S001100','S001',N'公司编号',N'1001',0,0.00,N'01','N','S',2003);
insert into SC_Config values('S001101','S001',N'公司名称',N'张家港富耐特',0,0.00,N'张家港富耐特','N','S',2004);
insert into SC_Config values('S001102','S001',N'公司地址',N'新奥',0,0.00,N'张家港杨舍镇晨新路19号','N','S',2007);
insert into SC_Config values('S001103','S001',N'服务热线',N'0512-56372',0,0.00,N'','N','S',1156);
insert into SC_Config values('S001104','S001',N'站点编号',N'10',0,0.00,N'0001','N','S',5005);
insert into SC_Config values('S001105','S001',N'站点名称',N'张家港城北',0,0.00,N'0001','N','S',5006);
insert into SC_Config values('S001106','S001',N'省级编码',N'24',0,0.00,N'21','N','S',1157);
insert into SC_Config values('S001107','S001',N'区域号',N'0912',0,0.00,N'1538','N','S',2140);
insert into SC_Config values('S001108','S001',N'本地车牌',N'陕K',0,0.00,N'川A','N','S',2284);
GO

insert into SC_Config values('S001109','S001',N'基础卡类型',N'',0,0.00,N'','Y','S',2285);
insert into SC_Config values('S001109101','S001109',N'员工卡',N'B1',0,0.00,N'B1','Y','S',2209);
insert into SC_Config values('S001109102','S001109',N'交班卡',N'BA',0,0.00,N'BA','Y','S',2210);
insert into SC_Config values('S001109103','S001109',N'管理卡',N'BB',0,0.00,N'BB','Y','S',2211);
insert into SC_Config values('S001109104','S001109',N'现金卡',N'C1',0,0.00,N'C1','Y','S',2212);
insert into SC_Config values('S001109105','S001109',N'记账卡',N'C2',0,0.00,N'C2','Y','S',2213);
insert into SC_Config values('S001109106','S001109',N'厂家维护卡',N'D1',0,0.00,N'D1','Y','S',2447);
GO

insert into SC_Config values('S001110','S001',N'基础车辆类型',N'',0,0.00,N'','Y','S',2286);
insert into SC_Config values('S001110101','S001110',N'出租车',N'01',0,0.00,N'01','Y','S',2029);
insert into SC_Config values('S001110102','S001110',N'公交车',N'02',0,0.00,N'02','Y','S',2030);
insert into SC_Config values('S001110103','S001110',N'中巴车',N'03',0,0.00,N'03','Y','S',2031);
insert into SC_Config values('S001110104','S001110',N'单位车',N'04',0,0.00,N'04','Y','S',2032);
insert into SC_Config values('S001110105','S001110',N'私家车',N'05',0,0.00,N'05','Y','S',2208);
insert into SC_Config values('S001110106','S001110',N'其它车',N'06',0,0.00,N'06','Y','S',2287);
GO

insert into SC_Config values('S001111','S001',N'基础单价类型',N'',0,0.00,N'','Y','S',2288);
insert into SC_Config values('S001111101','S001111',N'00',N'',0,4.00,N'','Y','F',2385);
insert into SC_Config values('S001111102','S001111',N'01',N'',0,4.50,N'','Y','F',2386);
insert into SC_Config values('S001111103','S001111',N'02',N'',0,5.00,N'','Y','F',2387);
GO

insert into SC_Config values('S001112','S001',N'基础消费方式',N'',0,0.00,N'','Y','S',2289);
insert into SC_Config values('S001112101','S001112',N'现金结算',N'00',0,0.00,N'00','Y','S',2214);
insert into SC_Config values('S001112102','S001112',N'预购气量',N'01',0,0.00,N'01','Y','S',2215);
insert into SC_Config values('S001112103','S001112',N'预购金额',N'02',0,0.00,N'02','Y','S',2216);
insert into SC_Config values('S001112104','S001112',N'记帐气量',N'03',0,0.00,N'03','Y','S',2217);
insert into SC_Config values('S001112105','S001112',N'记帐金额',N'04',0,0.00,N'04','Y','S',2218);
GO

insert into SC_Config values('S001113','S001',N'IC卡基本状态',N'',0,0.00,N'','Y','S',2290);
insert into SC_Config values('S001113101','S001113',N'正常',N'00',0,0.00,N'00','Y','I',2147);
insert into SC_Config values('S001113102','S001113',N'消费灰卡',N'',1,0.00,N'01','Y','I',2150);
insert into SC_Config values('S001113103','S001113',N'黑卡',N'',2,0.00,N'02','Y','I',2148);
insert into SC_Config values('S001113104','S001113',N'充值灰卡',N'',4,0.00,N'04','Y','I',2149);
insert into SC_Config values('S001113105','S001113',N'已挂失',N'',8,0.00,N'08','Y','I',2291);
insert into SC_Config values('S001113106','S001113',N'已补卡',N'',16,0.00,N'16','Y','I',2292);
insert into SC_Config values('S001113107','S001113',N'已注销',N'',32,0.00,N'32','Y','I',2293);
GO

insert into SC_Config values('S001114','S001',N'基础燃料类型',N'',0,0.00,N'','Y','S',2294);
insert into SC_Config values('S001114101','S001114',N'CNG',N'00',0,0.00,N'00','Y','S',2389);
insert into SC_Config values('S001114102','S001114',N'LNG',N'01',0,0.00,N'01','Y','S',2388);
GO

insert into SC_Config values('S001115','S001',N'遗失卡处理方式',N'',0,0.00,N'','Y','S',2295);
insert into SC_Config values('S001115101','S001115',N'口头',N'0',0,0.00,N'0','Y','S',2296);
insert into SC_Config values('S001115102','S001115',N'电话',N'0',0,0.00,N'0','Y','S',2297);
insert into SC_Config values('S001115103','S001115',N'柜台',N'0',0,0.00,N'0','Y','S',2298);
insert into SC_Config values('S001115104','S001115',N'网上',N'0',0,0.00,N'0','Y','S',2299);
GO

insert into SC_Config values('S001116','S001',N'信息版本号',N'',0,0.00,N'','Y','S',2300);
insert into SC_Config values('S001116101','S001116',N'黑名单版本号',N'',13,0.00,N'0','Y','I',2301);
insert into SC_Config values('S001116102','S001116',N'白名单版本号',N'',1,0.00,N'0','Y','I',2302);
insert into SC_Config values('S001116103','S001116',N'新删黑名单版本号',N'',2,0.00,N'0','Y','I',2303);
insert into SC_Config values('S001116104','S001116',N'新增黑名单版本号',N'',4,0.00,N'0','Y','I',2453);
insert into SC_Config values('S001116105','S001116',N'气价版本号',N'',0,0.00,N'0','Y','I',9070);
insert into SC_Config values('S001116106','S001116',N'通用信息版本号',N'',0,0.00,N'0','Y','I',9069);
insert into SC_Config Values('S001116107','S001116',N'CNG气价版本号',N'0',0,0.00,N'0','Y','I',9071);
insert into SC_Config Values('S001116108','S001116',N'LNG气价版本号',N'0',0,0.00,N'0','Y','I',9072);
insert into SC_Config Values('S001116109','S001116',N'LPG气价版本号',N'0',0,0.00,N'0','Y','I',9073);
GO

insert into SC_Config values('S001117','S001',N'证件类型',N'',0,0.00,N'','Y','S',2036);
insert into SC_Config values('S001117101','S001117',N'身份证',N'01',0,0.00,N'身份证','Y','S',2041);
insert into SC_Config values('S001117102','S001117',N'警官证',N'02',0,0.00,N'警官证','Y','S',2042);
insert into SC_Config values('S001117103','S001117',N'驾驶证',N'03',0,0.00,N'驾驶证','Y','S',2304);

insert into SC_Config values('S001118','S001',N'工本费',N'',0,10.00,N'10.00','N','F',2129);
insert into SC_Config values('S002101','S002',N'发卡器串口',N'',3,0.00,N'1','N','I',2306);
insert into SC_Config values('S002102','S002',N'小票打印机串口',N'',4,0.00,N'2','N','I',2307);
insert into SC_Config values('S002103','S002',N'波特率',N'',9600,0.00,N'9600','N','I',3006);
insert into SC_Config values('S002104','S002',N'校验位',N'',8,0.00,N'0','N','I',3007);
insert into SC_Config values('S002105','S002',N'数据位',N'',2,0.00,N'0','N','I',3008);
insert into SC_Config values('S002106','S002',N'停止位',N'',1,0.00,N'0','N','I',3009);
GO

insert into SC_Config values('S003101','S003',N'系统模式',N'',0,0.00,N'','Y','S',2309);
insert into SC_Config values('S003101101','S003101',N'单站模式',N'Y',0,0.00,N'Y','S','S',2310);
insert into SC_Config values('S003101102','S003101',N'联网模式',N'N',0,0.00,N'N','S','S',2311);
insert into SC_Config values('S003101103','S003101',N'银行模式',N'N',0,0.00,N'N','S','S',2312);
GO

insert into SC_Config values('S003102','S003',N'登录验证模式',N'',0,0.00,N'','Y','S',2313);
insert into SC_Config values('S003102101','S003102',N'IC卡登录',N'N',0,0.00,N'N','S','S',2314);
insert into SC_Config values('S003102102','S003102',N'普通登录',N'Y',0,0.00,N'Y','S','S',2315);
insert into SC_Config values('S003103','S003',N'充值模式',N'',0,0.00,N'','Y','S',2316);
insert into SC_Config values('S003103101','S003103',N'充值上限',N'',10000,0.00,N'10000','N','I',2317);
insert into SC_Config values('S003103102','S003103',N'充值下限',N'',10,0.00,N'10','N','I',2318);
insert into SC_Config values('S003103103','S003103',N'是否允许充入负值',N'N',0,0.00,N'N','S','S',2319);
GO

insert into SC_Config values('S003104','S003',N'每日加气次数限制',N'',0,0.00,N'99999','N','I',2320);
insert into SC_Config values('S003105','S003',N'钢瓶到期天数',N'',10,0.00,N'10','N','I',2321);
insert into SC_Config values('S003106','S003',N'是否启用小票打印',N'',0,0.00,N'Y','S','S',2322);
insert into SC_Config values('S003107','S003',N'是否计算折扣率',N'N',0,0.00,N'N','S','S',2323);
insert into SC_Config values('S003107101','S003107',N'折扣率',N'',0,0.20,N'0','N','F',2324);
GO

insert into SC_Config values('S003108','S003',N'是否启用优惠规则',N'N',0,0.00,N'N','S','S',2325);
insert into SC_Config values('S003108101','S003108',N'优惠规则启用方式',N'分段值',0,0.00,N'百分比','S','S',2326);
insert into SC_Config values('S003109','S003',N'初始密码',N'',888888,0.00,N'888888','N','I',1074);
GO

insert into SC_Config values('S003110','S003',N'IC卡挂失方式',N'',0,0.00,N'','Y','S',2327);
insert into SC_Config values('S003110101','S003110',N'柜台挂失',N'00',0,0.00,N'00','Y','S',2328);
insert into SC_Config values('S003110102','S003110',N'电话挂失',N'01',0,0.00,N'01','Y','S',2329);
insert into SC_Config values('S003110103','S003110',N'网络挂失',N'02',0,0.00,N'02','Y','S',2330);
GO

insert into SC_Config values('S003111','S003',N'是否启用电子标签',N'N',0,0.00,N'N','S','S',2331);
GO
