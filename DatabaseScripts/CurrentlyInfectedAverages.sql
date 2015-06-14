
select TOP 1 * from NetworkInfo
--select count(*) from Simulation

DECLARE @Runs INT;
DECLARE @vUsers INT;
DECLARE @nvUsers INT;

DECLARE @vWebsites INT;
DECLARE @nvWebsites INT;

SELECT @Runs = COUNT(*) FROM Simulation;
SELECT @Runs AS N;

SELECT @vUsers = VigilantUsers FROM NetworkInfo;
SELECT @nvUsers = NonVigilantUsers FROM NetworkInfo;
SELECT @vWebsites = VigilantWebsites FROM NetworkInfo;
SELECT @nvWebsites = NonVigilantWebsites FROM NetworkInfo;

--Display the general info:
SELECT @vUsers AS VUsers, @nvUsers AS NVUsers, @vWebsites AS VWebsites, @nvWebsites AS NVWEbsites

select	TurnNo
		--Vigilant Users
		, AVG(CAST(VigilantUsersInfectedTotal as float) - VigilantUsersRecoveredTotal) as CurrentlyInfectedVUAvg
		, 100 * AVG(CAST(VigilantUsersInfectedTotal as float) - VigilantUsersRecoveredTotal) / @vUsers AS CurrentlyInfectedVUAvgPercent
		--Non Vigilant Users
		, AVG(CAST(NonVigilantUsersInfectedTotal as float) - NonVigilantUsersRecoveredTotal) as CurrentlyInfectedNVUAvg
		, 100 * AVG(CAST(NonVigilantUsersInfectedTotal as float) - NonVigilantUsersRecoveredTotal) / @nvUsers AS CurrentlyInfectedNVUAvgPercent
		--Vigilant Websites
		, AVG(CAST(VigilantWebsitesInfectedTotal as float) - VigilantWebsitesRecoveredTotal) as CurrentlyInfectedVWAvg
		, 100 * AVG(CAST(VigilantWebsitesInfectedTotal as float) - VigilantWebsitesRecoveredTotal) / @vWebsites AS CurrentlyInfectedVWAvgPercent
		--Non Vigilant Websites
		,AVG(NonVigilantWebsitesInfectedTotal) AS NVWITotal, AVG(NonVigilantWebsitesRecoveredTotal) as NVWRTotal
		, AVG(CAST(NonVigilantWebsitesInfectedTotal as float) - NonVigilantWebsitesRecoveredTotal) as CurrentlyInfectedNVWAvg
		, 100 * AVG(CAST(NonVigilantWebsitesInfectedTotal as float) - NonVigilantWebsitesRecoveredTotal) / @nvWebsites AS CurrentlyInfectedNVWAvgPercent
from SimulationTurn st
group by TurnNo--, st.SimulationId
order by TurnNo ASC


