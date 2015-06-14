--MEAN INFECTED TOTAL

--MEANS
SELECT 
	AVG(CAST(VigilantUsersInfectedTotal AS FLOAT)) AS VUTotalMean,
	AVG(CAST(NonVigilantUsersInfectedTotal AS FLOAT)) AS NVUTotalVUTotalMean,
	AVG(CAST(VigilantWebsitesInfectedTotal AS FLOAT)) AS VWTotalVUTotalMean,
	AVG(CAST(NonVigilantWebsitesInfectedTotal AS FLOAT)) AS NVWTotalVUTotalMean
FROM SimulationTurn
WHERE TurnNo = 150

--MEDIANS
SELECT
(
 (SELECT MAX(VigilantUsersInfectedTotal) FROM
   (SELECT TOP 50 PERCENT VigilantUsersInfectedTotal FROM SimulationTurn ORDER BY VigilantUsersInfectedTotal) AS BottomHalf)
 +
 (SELECT MIN(VigilantUsersInfectedTotal) FROM
   (SELECT TOP 50 PERCENT VigilantUsersInfectedTotal FROM SimulationTurn ORDER BY VigilantUsersInfectedTotal DESC) AS TopHalf)
) / 2 AS VUTotalMedian,

(
 (SELECT MAX(NonVigilantUsersInfectedTotal) FROM
   (SELECT TOP 50 PERCENT NonVigilantUsersInfectedTotal FROM SimulationTurn ORDER BY NonVigilantUsersInfectedTotal) AS BottomHalf)
 +
 (SELECT MIN(NonVigilantUsersInfectedTotal) FROM
   (SELECT TOP 50 PERCENT NonVigilantUsersInfectedTotal FROM SimulationTurn ORDER BY NonVigilantUsersInfectedTotal DESC) AS TopHalf)
) / 2 AS NVUTotalMedian,




(
 (SELECT MAX(VigilantWebsitesInfectedTotal) FROM
   (SELECT TOP 50 PERCENT VigilantWebsitesInfectedTotal FROM SimulationTurn ORDER BY VigilantWebsitesInfectedTotal) AS BottomHalf)
 +
 (SELECT MIN(VigilantWebsitesInfectedTotal) FROM
   (SELECT TOP 50 PERCENT VigilantWebsitesInfectedTotal FROM SimulationTurn ORDER BY VigilantWebsitesInfectedTotal DESC) AS TopHalf)
) / 2 AS VWTotalMedian,

(
 (SELECT MAX(NonVigilantWebsitesInfectedTotal) FROM
   (SELECT TOP 50 PERCENT NonVigilantWebsitesInfectedTotal FROM SimulationTurn ORDER BY NonVigilantWebsitesInfectedTotal) AS BottomHalf)
 +
 (SELECT MIN(NonVigilantWebsitesInfectedTotal) FROM
   (SELECT TOP 50 PERCENT NonVigilantWebsitesInfectedTotal FROM SimulationTurn ORDER BY NonVigilantWebsitesInfectedTotal DESC) AS TopHalf)
) / 2 AS NVWTotalMedian