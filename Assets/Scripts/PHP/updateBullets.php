<?PHP 


require_once('functions.php');
require_once('database.php');




    $POSTid = input($_POST['id']);
    $POSTprice= input($_POST['price']);
    $POSTcantBullets = input($_POST['cantBullets']);
    $POSTweapponID = input($_POST['weapponID']);

    try {

        $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

        $stmt = $db->prepare("SELECT coins, uzibullets, shotgunbullets  FROM ".TAB_ACCOUNTS." WHERE id = :fuserId");
    
        $stmt->execute(array("fuserId" => "$POSTid"));

        $row = $stmt->fetch();
        
        $dbCoins = $row['coins'];
        $dbUziBullets = $row['uzibullets'];
        $dbShotgunBullets = $row['shotgunbullets'];


        $fixedCoins = $dbCoins -  $POSTprice;


       if($POSTweapponID == 1){

            $fixedUziBullets =  $dbUziBullets + $POSTcantBullets;
            $fixedShotgunBullets = $dbShotgunBullets;

             $stmt = $db->prepare("UPDATE ".TAB_ACCOUNTS." SET coins = :fcoins, uzibullets = :fuzibullets WHERE id = :fid");
            
            $stmt->execute(array(":fcoins" => "$fixedCoins",
                            ":fuzibullets" => "$fixedUziBullets",
                            ":fid" => "$POSTid"));

            $POSTweapponID = -1;
            die("COINS:$fixedCoins|USZIBULLETS:$fixedUziBullets|SHOTGUNBULLETS:$fixedShotgunBullets|MESSAGE:Buy successfully!");

       }
       elseif ($POSTweapponID == 2) {

            $fixedUziBullets =  $dbUziBullets;
            $fixedShotgunBullets = $dbShotgunBullets + $POSTcantBullets;

            $stmt = $db->prepare("UPDATE ".TAB_ACCOUNTS." SET coins = :fcoins, shotgunbullets = :fshotgunbullets WHERE id = :fid");
            
            $stmt->execute(array(":fcoins" => "$fixedCoins",
                            ":fshotgunbullets" => "$fixedShotgunBullets",
                            ":fid" => "$POSTid"));

            $POSTweapponID = -1;
            die("COINS:$fixedCoins|USZIBULLETS:$fixedUziBullets|SHOTGUNBULLETS:$fixedShotgunBullets|MESSAGE:Buy successfully!");
       }

    }
    catch(PDOException $e)
    {
        die("An error occurred, please retry.");
    }
    

?>