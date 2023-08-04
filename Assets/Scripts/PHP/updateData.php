<?PHP 


require_once('functions.php');
require_once('database.php');


    $POSTid = input($_POST['id']);
    $POSTscore =input($_POST['score']);
    $POSTcoins =input($_POST['coins']);


    try {

        $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

        $stmt = $db->prepare("SELECT score, coins FROM ".TAB_ACCOUNTS." WHERE id = :fuserId");
    
        $stmt->execute(array("fuserId" => "$POSTid"));

        $row = $stmt->fetch();
        
        $dbScore = $row['score'];      
        $dbCoins = $row['coins'];  

        $fixedScore = $dbScore + $POSTscore;
        $fixedCoins = $dbCoins + $POSTcoins;

        $stmt = $db->prepare("UPDATE ".TAB_ACCOUNTS." SET score = :fscore, coins = :fcoins WHERE id = :fid");
            
        $stmt->execute(array(":fscore" => "$fixedScore",
                        ":fcoins" => "$fixedCoins",
                        ":fid" => "$POSTid"));

        die("COINS:$fixedCoins|SCORE:$fixedScore|MESSAGE:Update successfully!");

    }
    catch(PDOException $e)
    {
        die("An error occurred, please retry.");
    }
    

?>