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



DECLARE @total INT
SELECT @total = VigilantWebsites FROM NetworkInfo;
select @total as Total;

SELECT COUNT(SimulationId) AS N FROM Simulation;

--MEAN INFECTED TOTAL
SELECT AVG(CAST(a.VigilantWebsitesInfectedTotal AS FLOAT)) AS Mean 
FROM
(
SELECT TOP 1 * 
FROM SimulationTurn
WHERE TurnNo = 150
ORDER BY VigilantWebsitesInfectedTotal ASC
)a;

--DISTRIBUTION % INFECTED TOTAL
SELECT
	SUM("0-1") AS "0-1",
	SUM("1-2") AS "1-2",
	SUM("2-3") AS "2-3",
	SUM("3-4") AS "3-4",
	SUM("4-5") AS "4-5",
	SUM("5-6") AS "5-6",
	SUM("6-7") AS "6-7",
	SUM("7-8") AS "7-8",
	SUM("8-9") AS "8-9",
	SUM("9-10") AS "9-10",
	SUM("10-11") AS "10-11",
	SUM("11-12") AS "11-12",
	SUM("12-13") AS "12-13",
	SUM("13-14") AS "13-14",
	SUM("14-15") AS "14-15",
	SUM("15-16") AS "15-16",
	SUM("16-17") AS "16-17",
	SUM("17-18") AS "17-18",
	SUM("18-19") AS "18-19",
	SUM("19-20") AS "19-20",
	SUM("20-21") AS "20-21",
	SUM("21-22") AS "21-22",
	SUM("22-23") AS "22-23",
	SUM("23-24") AS "23-24",
	SUM("24-25") AS "24-25",
	SUM("25-26") AS "25-26",
	SUM("26-27") AS "26-27",
	SUM("27-28") AS "27-28",
	SUM("28-29") AS "28-29",
	SUM("29-30") AS "29-30",
	SUM("30-31") AS "30-31",
	SUM("31-32") AS "31-32",
	SUM("32-33") AS "32-33",
	SUM("33-34") AS "33-34",
	SUM("34-35") AS "34-35",
	SUM("35-36") AS "35-36",
	SUM("36-37") AS "36-37",
	SUM("37-38") AS "37-38",
	SUM("38-39") AS "38-39",
	SUM("39-40") AS "39-40",
	SUM("40-41") AS "40-41",
	SUM("41-42") AS "41-42",
	SUM("42-43") AS "42-43",
	SUM("43-44") AS "43-44",
	SUM("44-45") AS "44-45",
	SUM("45-46") AS "45-46",
	SUM("46-47") AS "46-47",
	SUM("47-48") AS "47-48",
	SUM("48-49") AS "48-49",
	SUM("49-50") AS "49-50",
	SUM("50-51") AS "50-51",
	SUM("51-52") AS "51-52",
	SUM("52-53") AS "52-53",
	SUM("53-54") AS "53-54",
	SUM("54-55") AS "54-55",
	SUM("55-56") AS "55-56",
	SUM("56-57") AS "56-57",
	SUM("57-58") AS "57-58",
	SUM("58-59") AS "58-59",
	SUM("59-60") AS "59-60",
	SUM("60-61") AS "60-61",
	SUM("61-62") AS "61-62",
	SUM("62-63") AS "62-63",
	SUM("63-64") AS "63-64",
	SUM("64-65") AS "64-65",
	SUM("65-66") AS "65-66",
	SUM("66-67") AS "66-67",
	SUM("67-68") AS "67-68",
	SUM("68-69") AS "68-69",
	SUM("69-70") AS "69-70",
	SUM("70-71") AS "70-71",
	SUM("71-72") AS "71-72",
	SUM("72-73") AS "72-73",
	SUM("73-74") AS "73-74",
	SUM("74-75") AS "74-75",
	SUM("75-76") AS "75-76",
	SUM("76-77") AS "76-77",
	SUM("77-78") AS "77-78",
	SUM("78-79") AS "78-79",
	SUM("79-80") AS "79-80",
	SUM("80-81") AS "80-81",
	SUM("81-82") AS "81-82",
	SUM("82-83") AS "82-83",
	SUM("83-84") AS "83-84",
	SUM("84-85") AS "84-85",
	SUM("85-86") AS "85-86",
	SUM("86-87") AS "86-87",
	SUM("87-88") AS "87-88",
	SUM("88-89") AS "88-89",
	SUM("89-90") AS "89-90",
	SUM("90-91") AS "90-91",
	SUM("91-92") AS "91-92",
	SUM("92-93") AS "92-93",
	SUM("93-94") AS "93-94",
	SUM("94-95") AS "94-95",
	SUM("95-96") AS "95-96",
	SUM("96-97") AS "96-97",
	SUM("97-98") AS "97-98",
	SUM("98-99") AS "98-99",
	SUM("99-100") AS "99-100"
FROM
(

SELECT
	CASE WHEN Average > 0 AND Average <= 1 THEN 1 ELSE 0 END AS "0-1",
	CASE WHEN Average > 1 AND Average <= 2 THEN 1 ELSE 0 END AS "1-2",
	CASE WHEN Average > 2 AND Average <= 3 THEN 1 ELSE 0 END AS "2-3",
	CASE WHEN Average > 3 AND Average <= 4 THEN 1 ELSE 0 END AS "3-4",
	CASE WHEN Average > 4 AND Average <= 5 THEN 1 ELSE 0 END AS "4-5",
	CASE WHEN Average > 5 AND Average <= 6 THEN 1 ELSE 0 END AS "5-6",
	CASE WHEN Average > 6 AND Average <= 7 THEN 1 ELSE 0 END AS "6-7",
	CASE WHEN Average > 7 AND Average <= 8 THEN 1 ELSE 0 END AS "7-8",
	CASE WHEN Average > 8 AND Average <= 9 THEN 1 ELSE 0 END AS "8-9",
	CASE WHEN Average > 9 AND Average <= 10 THEN 1 ELSE 0 END AS "9-10",
	CASE WHEN Average > 10 AND Average <= 11 THEN 1 ELSE 0 END AS "10-11",
	CASE WHEN Average > 11 AND Average <= 12 THEN 1 ELSE 0 END AS "11-12",
	CASE WHEN Average > 12 AND Average <= 13 THEN 1 ELSE 0 END AS "12-13",
	CASE WHEN Average > 13 AND Average <= 14 THEN 1 ELSE 0 END AS "13-14",
	CASE WHEN Average > 14 AND Average <= 15 THEN 1 ELSE 0 END AS "14-15",
	CASE WHEN Average > 15 AND Average <= 16 THEN 1 ELSE 0 END AS "15-16",
	CASE WHEN Average > 16 AND Average <= 17 THEN 1 ELSE 0 END AS "16-17",
	CASE WHEN Average > 17 AND Average <= 18 THEN 1 ELSE 0 END AS "17-18",
	CASE WHEN Average > 18 AND Average <= 19 THEN 1 ELSE 0 END AS "18-19",
	CASE WHEN Average > 19 AND Average <= 20 THEN 1 ELSE 0 END AS "19-20",
	CASE WHEN Average > 20 AND Average <= 21 THEN 1 ELSE 0 END AS "20-21",
	CASE WHEN Average > 21 AND Average <= 22 THEN 1 ELSE 0 END AS "21-22",
	CASE WHEN Average > 22 AND Average <= 23 THEN 1 ELSE 0 END AS "22-23",
	CASE WHEN Average > 23 AND Average <= 24 THEN 1 ELSE 0 END AS "23-24",
	CASE WHEN Average > 24 AND Average <= 25 THEN 1 ELSE 0 END AS "24-25",
	CASE WHEN Average > 25 AND Average <= 26 THEN 1 ELSE 0 END AS "25-26",
	CASE WHEN Average > 26 AND Average <= 27 THEN 1 ELSE 0 END AS "26-27",
	CASE WHEN Average > 27 AND Average <= 28 THEN 1 ELSE 0 END AS "27-28",
	CASE WHEN Average > 28 AND Average <= 29 THEN 1 ELSE 0 END AS "28-29",
	CASE WHEN Average > 29 AND Average <= 30 THEN 1 ELSE 0 END AS "29-30",
	CASE WHEN Average > 30 AND Average <= 31 THEN 1 ELSE 0 END AS "30-31",
	CASE WHEN Average > 31 AND Average <= 32 THEN 1 ELSE 0 END AS "31-32",
	CASE WHEN Average > 32 AND Average <= 33 THEN 1 ELSE 0 END AS "32-33",
	CASE WHEN Average > 33 AND Average <= 34 THEN 1 ELSE 0 END AS "33-34",
	CASE WHEN Average > 34 AND Average <= 35 THEN 1 ELSE 0 END AS "34-35",
	CASE WHEN Average > 35 AND Average <= 36 THEN 1 ELSE 0 END AS "35-36",
	CASE WHEN Average > 36 AND Average <= 37 THEN 1 ELSE 0 END AS "36-37",
	CASE WHEN Average > 37 AND Average <= 38 THEN 1 ELSE 0 END AS "37-38",
	CASE WHEN Average > 38 AND Average <= 39 THEN 1 ELSE 0 END AS "38-39",
	CASE WHEN Average > 39 AND Average <= 40 THEN 1 ELSE 0 END AS "39-40",
	CASE WHEN Average > 40 AND Average <= 41 THEN 1 ELSE 0 END AS "40-41",
	CASE WHEN Average > 41 AND Average <= 42 THEN 1 ELSE 0 END AS "41-42",
	CASE WHEN Average > 42 AND Average <= 43 THEN 1 ELSE 0 END AS "42-43",
	CASE WHEN Average > 43 AND Average <= 44 THEN 1 ELSE 0 END AS "43-44",
	CASE WHEN Average > 44 AND Average <= 45 THEN 1 ELSE 0 END AS "44-45",
	CASE WHEN Average > 45 AND Average <= 46 THEN 1 ELSE 0 END AS "45-46",
	CASE WHEN Average > 46 AND Average <= 47 THEN 1 ELSE 0 END AS "46-47",
	CASE WHEN Average > 47 AND Average <= 48 THEN 1 ELSE 0 END AS "47-48",
	CASE WHEN Average > 48 AND Average <= 49 THEN 1 ELSE 0 END AS "48-49",
	CASE WHEN Average > 49 AND Average <= 50 THEN 1 ELSE 0 END AS "49-50",
	CASE WHEN Average > 50 AND Average <= 51 THEN 1 ELSE 0 END AS "50-51",
	CASE WHEN Average > 51 AND Average <= 52 THEN 1 ELSE 0 END AS "51-52",
	CASE WHEN Average > 52 AND Average <= 53 THEN 1 ELSE 0 END AS "52-53",
	CASE WHEN Average > 53 AND Average <= 54 THEN 1 ELSE 0 END AS "53-54",
	CASE WHEN Average > 54 AND Average <= 55 THEN 1 ELSE 0 END AS "54-55",
	CASE WHEN Average > 55 AND Average <= 56 THEN 1 ELSE 0 END AS "55-56",
	CASE WHEN Average > 56 AND Average <= 57 THEN 1 ELSE 0 END AS "56-57",
	CASE WHEN Average > 57 AND Average <= 58 THEN 1 ELSE 0 END AS "57-58",
	CASE WHEN Average > 58 AND Average <= 59 THEN 1 ELSE 0 END AS "58-59",
	CASE WHEN Average > 59 AND Average <= 60 THEN 1 ELSE 0 END AS "59-60",
	CASE WHEN Average > 60 AND Average <= 61 THEN 1 ELSE 0 END AS "60-61",
	CASE WHEN Average > 61 AND Average <= 62 THEN 1 ELSE 0 END AS "61-62",
	CASE WHEN Average > 62 AND Average <= 63 THEN 1 ELSE 0 END AS "62-63",
	CASE WHEN Average > 63 AND Average <= 64 THEN 1 ELSE 0 END AS "63-64",
	CASE WHEN Average > 64 AND Average <= 65 THEN 1 ELSE 0 END AS "64-65",
	CASE WHEN Average > 65 AND Average <= 66 THEN 1 ELSE 0 END AS "65-66",
	CASE WHEN Average > 66 AND Average <= 67 THEN 1 ELSE 0 END AS "66-67",
	CASE WHEN Average > 67 AND Average <= 68 THEN 1 ELSE 0 END AS "67-68",
	CASE WHEN Average > 68 AND Average <= 69 THEN 1 ELSE 0 END AS "68-69",
	CASE WHEN Average > 69 AND Average <= 70 THEN 1 ELSE 0 END AS "69-70",
	CASE WHEN Average > 70 AND Average <= 71 THEN 1 ELSE 0 END AS "70-71",
	CASE WHEN Average > 71 AND Average <= 72 THEN 1 ELSE 0 END AS "71-72",
	CASE WHEN Average > 72 AND Average <= 73 THEN 1 ELSE 0 END AS "72-73",
	CASE WHEN Average > 73 AND Average <= 74 THEN 1 ELSE 0 END AS "73-74",
	CASE WHEN Average > 74 AND Average <= 75 THEN 1 ELSE 0 END AS "74-75",
	CASE WHEN Average > 75 AND Average <= 76 THEN 1 ELSE 0 END AS "75-76",
	CASE WHEN Average > 76 AND Average <= 77 THEN 1 ELSE 0 END AS "76-77",
	CASE WHEN Average > 77 AND Average <= 78 THEN 1 ELSE 0 END AS "77-78",
	CASE WHEN Average > 78 AND Average <= 79 THEN 1 ELSE 0 END AS "78-79",
	CASE WHEN Average > 79 AND Average <= 80 THEN 1 ELSE 0 END AS "79-80",
	CASE WHEN Average > 80 AND Average <= 81 THEN 1 ELSE 0 END AS "80-81",
	CASE WHEN Average > 81 AND Average <= 82 THEN 1 ELSE 0 END AS "81-82",
	CASE WHEN Average > 82 AND Average <= 83 THEN 1 ELSE 0 END AS "82-83",
	CASE WHEN Average > 83 AND Average <= 84 THEN 1 ELSE 0 END AS "83-84",
	CASE WHEN Average > 84 AND Average <= 85 THEN 1 ELSE 0 END AS "84-85",
	CASE WHEN Average > 85 AND Average <= 86 THEN 1 ELSE 0 END AS "85-86",
	CASE WHEN Average > 86 AND Average <= 87 THEN 1 ELSE 0 END AS "86-87",
	CASE WHEN Average > 87 AND Average <= 88 THEN 1 ELSE 0 END AS "87-88",
	CASE WHEN Average > 88 AND Average <= 89 THEN 1 ELSE 0 END AS "88-89",
	CASE WHEN Average > 89 AND Average <= 90 THEN 1 ELSE 0 END AS "89-90",
	CASE WHEN Average > 90 AND Average <= 91 THEN 1 ELSE 0 END AS "90-91",
	CASE WHEN Average > 91 AND Average <= 92 THEN 1 ELSE 0 END AS "91-92",
	CASE WHEN Average > 92 AND Average <= 93 THEN 1 ELSE 0 END AS "92-93",
	CASE WHEN Average > 93 AND Average <= 94 THEN 1 ELSE 0 END AS "93-94",
	CASE WHEN Average > 94 AND Average <= 95 THEN 1 ELSE 0 END AS "94-95",
	CASE WHEN Average > 95 AND Average <= 96 THEN 1 ELSE 0 END AS "95-96",
	CASE WHEN Average > 96 AND Average <= 97 THEN 1 ELSE 0 END AS "96-97",
	CASE WHEN Average > 97 AND Average <= 98 THEN 1 ELSE 0 END AS "97-98",
	CASE WHEN Average > 98 AND Average <= 99 THEN 1 ELSE 0 END AS "98-99",
	CASE WHEN Average > 99 AND Average <= 100 THEN 1 ELSE 0 END AS "99-100"
FROM
(
	--Use this line for percentages
	SELECT 100 * AVG(cast(VigilantWebsitesInfectedTotal as float))/@total AS Average	
	FROM SimulationTurn
	GROUP BY SimulationId
)a
)b

