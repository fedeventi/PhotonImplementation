<?PHP 


require_once('functions.php');
require_once('database.php');



    try {

        $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

        $stmt = $db->prepare("SELECT `username`, `score` FROM `accounts` ORDER BY `score` DESC ");
        
        $stmt->execute();
        $row = $stmt->rowCount();
        
        if($row <= 0)
            die("There are no Auctions currently, retry later.");
    
        while($row = $stmt->fetch(PDO::FETCH_ASSOC))
        {        
            echo("".$row['username']."|".$row['score'].";");
        }

        die();      
    

    }
    catch(PDOException $e)
    {
        die("An error occurred, please retry.");
    }
    

?>