<?php

define('SERVER_NAME', 'localhost');
define('DB_NAME', 'u593689141_dprdb');
define('DB_USER', 'u593689141_santih');
define('DB_PASS', 'H@ndel1894');

define('TAB_ACCOUNTS', 'accounts');


try
{
	$db = new PDO("mysql:host=".SERVER_NAME."; dbname=".DB_NAME."", DB_USER, DB_PASS);
	$db->setAttribute(PDO::ATTR_ERRMODE , PDO::ERRMODE_EXCEPTION);
	$db->exec('SET NAMES "utf8"');
}
catch (PDOException $e)
{
	echo "ERROR CONECTION TO DATABASE " . $e->getMessage();
	exit();
}


?>