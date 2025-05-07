-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: todolist
-- ------------------------------------------------------
-- Server version	8.0.41

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
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20250506214442_InitialCreate','8.0.4'),('20250506221132_FixToDoListRelationships','8.0.4');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `listshares`
--

DROP TABLE IF EXISTS `listshares`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `listshares` (
  `ToDoListId` int NOT NULL,
  `UserId` int NOT NULL,
  PRIMARY KEY (`ToDoListId`,`UserId`),
  KEY `IX_ListShares_UserId` (`UserId`),
  CONSTRAINT `FK_ListShares_ToDoLists_ToDoListId` FOREIGN KEY (`ToDoListId`) REFERENCES `todolists` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ListShares_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `listshares`
--

LOCK TABLES `listshares` WRITE;
/*!40000 ALTER TABLE `listshares` DISABLE KEYS */;
/*!40000 ALTER TABLE `listshares` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taskassignments`
--

DROP TABLE IF EXISTS `taskassignments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `taskassignments` (
  `TaskItemId` int NOT NULL,
  `UserId` int NOT NULL,
  PRIMARY KEY (`TaskItemId`,`UserId`),
  KEY `IX_TaskAssignments_UserId` (`UserId`),
  CONSTRAINT `FK_TaskAssignments_TaskItems_TaskItemId` FOREIGN KEY (`TaskItemId`) REFERENCES `taskitems` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_TaskAssignments_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taskassignments`
--

LOCK TABLES `taskassignments` WRITE;
/*!40000 ALTER TABLE `taskassignments` DISABLE KEYS */;
/*!40000 ALTER TABLE `taskassignments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taskitems`
--

DROP TABLE IF EXISTS `taskitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `taskitems` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ToDoListId` int NOT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsComplete` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CreatedById` int NOT NULL,
  `LastModifiedAt` datetime(6) NOT NULL,
  `LastModifiedById` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_TaskItems_CreatedById` (`CreatedById`),
  KEY `IX_TaskItems_LastModifiedById` (`LastModifiedById`),
  KEY `IX_TaskItems_ToDoListId` (`ToDoListId`),
  CONSTRAINT `FK_TaskItems_ToDoLists_ToDoListId` FOREIGN KEY (`ToDoListId`) REFERENCES `todolists` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_TaskItems_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `users` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_TaskItems_Users_LastModifiedById` FOREIGN KEY (`LastModifiedById`) REFERENCES `users` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taskitems`
--

LOCK TABLES `taskitems` WRITE;
/*!40000 ALTER TABLE `taskitems` DISABLE KEYS */;
INSERT INTO `taskitems` VALUES (7,7,'nesto1','1234',1,'2025-05-07 01:38:13.042131',2,'2025-05-07 01:38:19.981712',2),(8,7,'nesto2','3456',0,'2025-05-07 01:38:18.639190',2,'2025-05-07 01:38:18.639193',2),(9,7,'nesto3','5678\r\nfwagaw',1,'2025-05-07 01:38:28.986080',2,'2025-05-07 01:38:31.351890',2);
/*!40000 ALTER TABLE `taskitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `todolists`
--

DROP TABLE IF EXISTS `todolists`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `todolists` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreatedAt` datetime(6) NOT NULL,
  `CreatedById` int NOT NULL,
  `LastModifiedAt` datetime(6) NOT NULL,
  `LastModifiedById` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ToDoLists_CreatedById` (`CreatedById`),
  KEY `IX_ToDoLists_LastModifiedById` (`LastModifiedById`),
  CONSTRAINT `FK_ToDoLists_Users_CreatedById` FOREIGN KEY (`CreatedById`) REFERENCES `users` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ToDoLists_Users_LastModifiedById` FOREIGN KEY (`LastModifiedById`) REFERENCES `users` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `todolists`
--

LOCK TABLES `todolists` WRITE;
/*!40000 ALTER TABLE `todolists` DISABLE KEYS */;
INSERT INTO `todolists` VALUES (6,'nesto1','2025-05-07 01:37:49.202368',1,'2025-05-07 01:37:49.202370',1),(7,'nesto2','2025-05-07 01:38:04.652199',2,'2025-05-07 01:38:04.652201',2);
/*!40000 ALTER TABLE `todolists` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Username` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `FirstName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `LastName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreatedAt` datetime(6) NOT NULL,
  `CreatedBy` int NOT NULL,
  `LastModified` datetime(6) NOT NULL,
  `LastModifiedBy` int NOT NULL,
  `TaskItemId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Users_TaskItemId` (`TaskItemId`),
  CONSTRAINT `FK_Users_TaskItems_TaskItemId` FOREIGN KEY (`TaskItemId`) REFERENCES `taskitems` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','adminpass','Admin','User','2025-05-07 01:11:32.000000',1,'2025-05-07 01:11:32.000000',1,NULL),(2,'Mario','mario','Mario','Mario','2025-05-07 01:02:37.852127',1,'2025-05-07 01:02:37.852309',1,NULL);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-07  1:41:25
