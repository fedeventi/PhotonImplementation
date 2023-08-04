<?PHP 

function input($value) 
{

 	$value = trim($value);
 	$value = stripslashes($value);
 	$value = htmlspecialchars($value);
 	return $value;
}


function c_hash_password($password) 
{
	$options = 
	[
	    'cost' => 12,
	];
	return password_hash($password, PASSWORD_BCRYPT, $options);
}


function str_replace_first_match($from, $to, $content)
{
    $from = '/'.preg_quote($from, '/').'/';

    return preg_replace($from, $to, $content, 1);
}

?>