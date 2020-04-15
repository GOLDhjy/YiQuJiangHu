/*
SQLyog Ultimate v12.09 (64 bit)
MySQL - 5.5.54 : Database - yiqujianghu
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`yiqujianghu` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `yiqujianghu`;

/*Table structure for table `gameinfo` */

CREATE TABLE `gameinfo` (
  `Gid` int(11) NOT NULL AUTO_INCREMENT COMMENT '游戏ID',
  `Score` int(11) DEFAULT '0' COMMENT '分数',
  `Chapter` int(11) DEFAULT '1' COMMENT '关卡',
  `ID` bigint(20) NOT NULL COMMENT '用户ID',
  PRIMARY KEY (`Gid`,`ID`),
  KEY `ID` (`ID`),
  CONSTRAINT `gameinfo_ibfk_1` FOREIGN KEY (`ID`) REFERENCES `logininfo` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

/*Data for the table `gameinfo` */

insert  into `gameinfo`(`Gid`,`Score`,`Chapter`,`ID`) values (1,60,1,1);
insert  into `gameinfo`(`Gid`,`Score`,`Chapter`,`ID`) values (2,70,2,1);
insert  into `gameinfo`(`Gid`,`Score`,`Chapter`,`ID`) values (3,50,1,2);
insert  into `gameinfo`(`Gid`,`Score`,`Chapter`,`ID`) values (4,90,3,1);
insert  into `gameinfo`(`Gid`,`Score`,`Chapter`,`ID`) values (5,100,4,1);

/*Table structure for table `logininfo` */

CREATE TABLE `logininfo` (
  `ID` bigint(20) NOT NULL COMMENT '用户ID',
  `Name` varchar(50) NOT NULL COMMENT '姓名',
  `Password` varchar(50) NOT NULL COMMENT '密码',
  `Status` tinyint(1) DEFAULT '0' COMMENT '登录状态',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Data for the table `logininfo` */

insert  into `logininfo`(`ID`,`Name`,`Password`,`Status`) values (1,'jack','1234',0);
insert  into `logininfo`(`ID`,`Name`,`Password`,`Status`) values (2,'mary','1111',1);
insert  into `logininfo`(`ID`,`Name`,`Password`,`Status`) values (70741952397312,'lb','666',0);
insert  into `logininfo`(`ID`,`Name`,`Password`,`Status`) values (105926324748288,'xuzhimo','888',0);
insert  into `logininfo`(`ID`,`Name`,`Password`,`Status`) values (299440370343936,'Bob','111',0);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
