package smos.storage;

import java.sql.SQLException;
import java.util.Collection;

import smos.bean.Address;
import smos.bean.Teaching;
import smos.bean.UserListItem;
import smos.bean.Votes;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.exception.MandatoryFieldException;
import smos.storage.connectionManagement.exception.ConnectionException;

public class UserTest {

	/**
	 * @param args
	 * @throws InvalidValueException 
	 * @throws ConnectionException 
	 * @throws EntityNotFoundException 
	 * @throws SQLException 
	 * @throws MandatoryFieldException 
	 */
	public static void main(String[] args) throws SQLException, EntityNotFoundException, ConnectionException, InvalidValueException, MandatoryFieldException {
		
	UserListItem temp =new UserListItem();
	temp.setId(5);
	Teaching teaching = new Teaching();
	teaching.setId(3);
	ManagerVotes mv = ManagerVotes.getInstance();
	Votes ghh = new Votes();
	ghh.setAccademicYear(2134);
	ghh.setId_user(88);
	ghh.setLaboratory(3);
	ghh.setOral(4);
	ghh.setTeaching(9);
	ghh.setTurn(2);
	ghh.setWritten(3);
	mv.insert(ghh);
	}	
}
