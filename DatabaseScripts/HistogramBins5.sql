SELECT COUNT(SimulationId) AS N FROM Simulation;

SELECT
	SUM("0-5") AS "0-5",
	SUM("5-10") AS "5-10",
	SUM("10-15") AS "10-15",
	SUM("15-20") AS "15-20",
	SUM("20-25") AS "20-25",
	SUM("25-30") AS "25-30",
	SUM("30-35") AS "30-35",
	SUM("35-40") AS "35-40",
	SUM("40-45") AS "40-45",
	SUM("45-50") AS "45-50",
	SUM("50-55") AS "50-55",
	SUM("55-60") AS "55-60",
	SUM("60-65") AS "60-65",
	SUM("65-70") AS "65-70",
	SUM("70-75") AS "70-75",
	SUM("75-80") AS "75-80",
	SUM("80-85") AS "80-85",
	SUM("85-90") AS "85-90",
	SUM("90-95") AS "90-95",
	SUM("95-100") AS "95-100"
FROM
(

SELECT
	CASE WHEN Average > 0 AND Average <= 5 THEN 1 ELSE 0 END AS "0-5",
	CASE WHEN Average > 5 AND Average <= 10 THEN 1 ELSE 0 END AS "5-10",
	CASE WHEN Average > 10 AND Average <= 15 THEN 1 ELSE 0 END AS "10-15",
	CASE WHEN Average > 15 AND Average <= 20 THEN 1 ELSE 0 END AS "15-20",
	CASE WHEN Average > 20 AND Average <= 25 THEN 1 ELSE 0 END AS "20-25",
	CASE WHEN Average > 25 AND Average <= 30 THEN 1 ELSE 0 END AS "25-30",
	CASE WHEN Average > 30 AND Average <= 35 THEN 1 ELSE 0 END AS "30-35",
	CASE WHEN Average > 35 AND Average <= 40 THEN 1 ELSE 0 END AS "35-40",
	CASE WHEN Average > 40 AND Average <= 45 THEN 1 ELSE 0 END AS "40-45",
	CASE WHEN Average > 45 AND Average <= 50 THEN 1 ELSE 0 END AS "45-50",
	CASE WHEN Average > 50 AND Average <= 55 THEN 1 ELSE 0 END AS "50-55",
	CASE WHEN Average > 55 AND Average <= 60 THEN 1 ELSE 0 END AS "55-60",
	CASE WHEN Average > 60 AND Average <= 65 THEN 1 ELSE 0 END AS "60-65",
	CASE WHEN Average > 65 AND Average <= 70 THEN 1 ELSE 0 END AS "65-70",
	CASE WHEN Average > 70 AND Average <= 75 THEN 1 ELSE 0 END AS "70-75",
	CASE WHEN Average > 75 AND Average <= 80 THEN 1 ELSE 0 END AS "75-80",
	CASE WHEN Average > 80 AND Average <= 85 THEN 1 ELSE 0 END AS "80-85",
	CASE WHEN Average > 85 AND Average <= 90 THEN 1 ELSE 0 END AS "85-90",
	CASE WHEN Average > 90 AND Average <= 95 THEN 1 ELSE 0 END AS "90-95",
	CASE WHEN Average > 95 AND Average <= 100 THEN 1 ELSE 0 END AS "95-100"
FROM
(
	SELECT AVG(cast(VigilantUsersInfected as float)) AS Average
	FROM SimulationTurn
	GROUP BY SimulationId
)a
)b

