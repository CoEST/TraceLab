package smos.storage;

import java.sql.SQLException;
import java.util.Collection;
import java.util.Date;
import java.util.GregorianCalendar;

import smos.bean.Absence;
import smos.bean.Delay;
import smos.bean.Justify;
import smos.bean.Note;
import smos.bean.RegisterLine;
import smos.exception.EntityNotFoundException;
import smos.exception.InvalidValueException;
import smos.storage.ManagerRegister;
import smos.storage.connectionManagement.exception.ConnectionException;
import smos.utility.Utility;


public class TestRegister {

	// database errato, impossibile inserire null nel campo id_justify di absence
	public static void main(String[] args) {
		
		ManagerRegister mr=  ManagerRegister.getInstance();
		
		Date datenow= new Date();
		
		Absence absence = new Absence();		
		absence.setIdUser(61);
		absence.setDateAbsence(datenow);
		absence.setIdJustify(0);
		absence.setAcademicYear(2009);
		//absence.setIdAbsence(13);
		
		/*
		try {
			absence= mr.getAbsenceByIdAbsence(12);
			
		} catch (InvalidValueException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		} catch (EntityNotFoundException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		} catch (ConnectionException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		} catch (SQLException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
		*/
		
		Justify justifynew= new Justify();
		justifynew.setIdUser(1);
		justifynew.setDateJustify(datenow);
		justifynew.setAcademicYear(2008);

		justifynew.setIdJustify(6);
		
		
		Delay delay = new Delay();
		//delay.setIdDelay(3);
		delay.setIdUser(62);
		delay.setDateDelay(datenow);
		delay.setTimeDelay("10:00:00");
		delay.setAcademicYear(2009);
		
		Note notenew= new Note();
		notenew.setIdUser(2);
		notenew.setTeacher("boh");
		notenew.setAcademicYear(2009);
		notenew.setDateNote(datenow);
		notenew.setDescription("lo studente dorme in classe");
		notenew.setIdNote(1);
		
		try{
			//mr.insertAbsence(absence);
			//if(mr.exists(absence)) System.out.println("esiste");;
			//mr.deleteAbsence(absence);
			//mr.updateAbsence(absence);
			
			mr.insertDelay(delay);
			//if(mr.exists(delay)) System.out.println("esiste");;
			//mr.updateDelay(delay);
			//mr.deleteDelay(delay);
			
			//mr.insertNote(notenew);
			//mr.deleteNote(notenew);
			
			//mr.insertJustify(justifynew, absence);
			//mr.deleteJustify(justifynew.getIdJustify());
			//if(mr.hasJustify(absence))System.out.println("giustificata");
			
			//absence= mr.getAbsenceByIDUserAndDate(2, "2009-05-18");
			//absence= mr.getAbsenceByIdJustify(1);
			//System.out.println(absence.getDateAbsence());
			
			//Collection<Absence> ac= mr.getAbsenceByIDUserAndAcademicYear(2, 2009);
			//for(Absence x : ac) System.out.println(x.getDateAbsence());
			
			//justifynew=mr.getJustifyByAbsence(absence);
			//System.out.println(justifynew.getDateJustify());
			
			//Collection<Note> nc = mr.getNoteByIDUserAndAcademicYear(2, 2009);
			//for(Note x : nc) System.out.println(x.getDescription());
			
			//delay= mr.getDelayByIDUserAndDate(1, datenow);
			//System.out.println(delay.getDateDelay());
			//System.out.println(delay.getTimeDelay());
			
			
			///*
			Collection<RegisterLine> crl = mr.getRegisterByClassIDAndDate(64, datenow );
			for(RegisterLine x : crl){
				System.out.println(x.getStudent().getName());
				if(mr.hasAbsence(x)){
					System.out.println("assente");
				}
				System.out.println(mr.hasAbsence(x));
				if(mr.hasDelay(x)){
					System.out.println("ritardo");
				}
			}
			//*/
			
			GregorianCalendar gc = new GregorianCalendar();
	
			String date="";
			int year=gc.get(GregorianCalendar.YEAR);
			
			int month=gc.get(GregorianCalendar.MONTH)+1;
			String months="";
			if(month<10){
				months="0"+month;
			}else{
				months= months+month;
			}
			
			int day=gc.get(GregorianCalendar.DAY_OF_MONTH);
			String days="";
			if(day<10){
				days="0"+day;
			}else{
				days= days+day;
			}
			
			date= date + year+"-"+months+"-"+days;
			System.out.println(date);
			
		}catch (Exception e) {
			System.out.println("errore");
			System.out.println(e.getMessage());
		}
		
		System.out.println(datenow.toString());
		System.out.println(Utility.isNull(datenow));
		System.out.println("TEST COMPLETATO");
	}
	
}
