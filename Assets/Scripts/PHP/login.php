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

    if($Username_Count == 0)
        die("Username does not exists.");

    // Find the Account ID
    $stmt = $db->prepare("SELECT id,username, score, coins, uzibullets, shotgunbullets FROM ".TAB_ACCOUNTS." WHERE `username`=:fuser");
    
    $stmt->execute(array("fuser" => "$POSTusername"));

    $row = $stmt->fetch();
    $AccountID = $row['id']; 
    $Username = $row['username']; 
    $Score = $row['score']; 
    $Coins = $row['coins'];
    $Uzibullets = $row['uzibullets'];
    $Shotgunbullets = $row['shotgunbullets'];


    $stmt = $db->prepare("SELECT password FROM ".TAB_ACCOUNTS." WHERE `username`=:fuser");
    
    $stmt->execute(array("fuser" => "$POSTusername"));

    $row = $stmt->fetch();

    $dbPassword = $row['password'];      


  


    if(password_verify($POSTpassword, $dbPassword)) 
    {


    die("ID:$AccountID|USERNAME:$Username|SCORE:$Score|COINS:$Coins|USZIBULLETS:$Uzibullets|SHOTGUNBULLETS:$Shotgunbullets|MESSAGE:Successfully logged in!\nLoading...");
    
    } 
    else 
    {
        die("Password does not match \n");
    }


    }
    catch(PDOException $e)
    {
        die("An error occurred, please retry.");
    }
    

?>