package DB;
import Bean.*;
/**
 * La classe DbException viene lanciata quando si verifica un eccezione legata al db
 * @author Antonio Leone
 * @version 1.0
 */
public class DbException extends RuntimeException {

	private static final long serialVersionUID = -6403170047487930045L;
	public DbException()
	{
	}
	public DbException(String message)
	{
		super(message);
	}
}