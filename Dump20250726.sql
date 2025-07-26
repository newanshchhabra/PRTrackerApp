CREATE DATABASE  IF NOT EXISTS `alcatel` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `alcatel`;
-- MySQL dump 10.13  Distrib 8.0.43, for Win64 (x86_64)
--
-- Host: localhost    Database: alcatel
-- ------------------------------------------------------
-- Server version	9.4.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `prtable`
--

DROP TABLE IF EXISTS `prtable`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prtable` (
  `pr` int unsigned NOT NULL,
  `summary` varchar(512) DEFAULT NULL,
  `manager_name` char(32) DEFAULT NULL,
  `engineer_name` char(32) DEFAULT NULL,
  `severity` enum('Critical','Major','Minor') NOT NULL,
  `status` enum('Canceled','Closed','Duplicate','Evaluating','Feedback','Informational','Monitoring','Open','Verify','Deleted','Deferred') NOT NULL,
  `tester_name` char(32) NOT NULL,
  `category` char(32) NOT NULL DEFAULT '',
  `sub_category` char(32) NOT NULL DEFAULT '',
  `meeting_minutes` char(32) NOT NULL,
  `pr_cr_date` datetime NOT NULL,
  `rtr` char(32) NOT NULL,
  `pr_active` enum('active','inactive') NOT NULL DEFAULT 'active',
  `action_owner` varchar(256) NOT NULL,
  `age` double unsigned NOT NULL DEFAULT '0',
  `engineering_comment` varchar(1024) NOT NULL,
  `num_blocking_tc` int NOT NULL DEFAULT '0',
  `pr_release` char(16) NOT NULL DEFAULT '7.3.4.R02',
  `pr_origin` enum('Testing Engineering','Customer Service','SQA','Engineering','Simulation','HQA','Manufacturing','Dev-AOS-Reuse','MIS','PT-AOS-Reuse','SQA-Delta') NOT NULL DEFAULT 'Engineering',
  `vf_product` char(32) NOT NULL,
  `blocking` enum('T','F') NOT NULL DEFAULT 'F',
  `pr_rca` enum('noRCA','minorCE','largeCE','minorDesign','majorDesign','merge','designComm','duplicate','existing','nofix','collateralDamage','scalability','user','invalidTest','unknown','linux','hw','other','reqUnclear','extern(BCM)','Deferred') NOT NULL DEFAULT 'noRCA',
  `pr_rcasummary` varchar(512) NOT NULL DEFAULT '',
  `pr_rca_action` enum('noAction','addUT','addPALTest','informReviewer','informArchitecture') NOT NULL DEFAULT 'noAction',
  `pr_rca_type` enum('Undefined','newCode','Existing','Broken','N/A') NOT NULL DEFAULT 'Undefined',
  `vtbf_age` double unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`pr`),
  KEY `pr_release` (`pr_release`),
  KEY `manager_index` (`manager_name`),
  KEY `release_index` (`pr_release`),
  KEY `blocking_index` (`blocking`,`pr_active`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prtable`
--

LOCK TABLES `prtable` WRITE;
/*!40000 ALTER TABLE `prtable` DISABLE KEYS */;
INSERT INTO `prtable` VALUES (1,'Example bug report','Jane','Bob','Major','Open','Alice','Software','UI','Sprint-10 Review','2025-07-25 21:30:18','RTR01','active','Bob',0.5,'Investigating',2,'7.3.4.R02','Engineering','Switch-X','T','minorDesign','Null pointer crash','addUT','newCode',1);
/*!40000 ALTER TABLE `prtable` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-26 11:25:13
