--INFECTED PER GO
--users
SELECT VigilantUsersInfected, COUNT(VigilantUsersInfected) AS Amount
FROM SimulationTurn
WHERE TurnNo = 150
GROUP BY VigilantUsersInfected
ORDER BY VigilantUsersInfected ASC

SELECT NonVigilantUsersInfected, COUNT(NonVigilantUsersInfected) AS Amount
FROM SimulationTurn
WHERE TurnNo = 150
GROUP BY NonVigilantUsersInfected
ORDER BY NonVigilantUsersInfected ASC

--websites
SELECT VigilantWebsitesInfected, COUNT(VigilantWebsitesInfected) AS Amount
FROM SimulationTurn
WHERE TurnNo = 150
GROUP BY VigilantWebsitesInfected
ORDER BY VigilantWebsitesInfected ASC

SELECT NonVigilantWebsitesInfected, COUNT(NonVigilantWebsitesInfected) AS Amount
FROM SimulationTurn
WHERE TurnNo = 150
GROUP BY NonVigilantWebsitesInfected
ORDER BY NonVigilantWebsitesInfected ASC
