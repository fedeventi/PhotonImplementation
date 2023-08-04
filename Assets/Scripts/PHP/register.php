<?PHP

require_once('functions.php');
require_once('database.php');




	if(!isset($_POST['username'], $_POST['password']) || 	
		empty(input($_POST['username'])) || empty(input($_POST['password']))) 	
			die("There was an error. Please retry."); 														



	if(preg_match("/^[A-Za-z0-9\.\_\-]{1,22}/", input($_POST['username']))) 
		$POSTusername = input($_POST['username']);
	else 
		die("There was an error. Please retry.");
	

	if(preg_match("/^[A-Za-z0-9\.\_\-]{1,32}/", input($_POST['password']))) 
		$POSTpassword = input($_POST['password']);		
	 else 
		die("There was an error. Please retry.");
 


	try {

    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    
    // CHECK USERNAME

    $stmt = $db->prepare("SELECT username FROM ".TAB_ACCOUNTS." WHERE `username`=:fuser");
    
    $stmt->execute(array("fuser" => "$POSTusername"));

    $Username_Count = $stmt->rowCount();



	
		if($Username_Count > 0)
			die("Username is already in use.");
	
		else		
		{

			$newPass = c_hash_password($POSTpassword);


	    	$stmt = $db->prepare("INSERT INTO ".TAB_ACCOUNTS." (username, password,score,coins)
    								  VALUES(:fuser, :fpassword, :fscore)");

			$stmt->execute(array(
					    "fuser" => "$POSTusername",
	                    "fpassword" => "$newPass",
	                    "fscore" => "0",
						"fcoins" => "100"));

	    	
			die ("Registration completed!");	

			}	
		}
		catch(PDOException $e)
		    {
		    	// echo "Error: " . $e->getMessage();
		    	die("An error occurred, please retry.");

		    }	



?>