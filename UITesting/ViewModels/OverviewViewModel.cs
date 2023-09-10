using System;
using OpenQA.Selenium;
using UITesting;
using UITesting.Pages;

namespace UITesting
{
    public class OverviewViewModel : BaseTestUI
    {
        public static IWebDriver _driver => Driver.Instance;

        public static string SiteName => "5365_Baltimore MD Distribution Site"; 

        //Shift

        public static DateTime TodaysDate => DateTime.Now;

        public static string ShiftName => $"M-{TodaysDate.ToString("MMddyy")}_RCD_Test_Route_Baltimore_1200_Su-Sa";

        public static IWebElement ShiftGrid
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[2]/table/tbody")); }
        }

        public static IWebElement ShiftSearchBar
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[1]/div/table/thead/tr[2]/td[2]/span/span/span/input[1]")); }
        }
        public static IWebElement ShiftTypeDropDown
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[1]/div/table/thead/tr[2]/td[2]/span/span/span")); }
        }
        public static IWebElement OrderButton
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[1]/div/table/thead/tr[1]/th[1]/span/span")); }
        }
        public static IWebElement DateSearchBar
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[1]/div/table/thead/tr[2]/td[3]/span/span/span/input")); }
        }
        public static IWebElement NumberOfMovementsSearchBar
        {
            get { return _driver.FindElement(By.XPath("//html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[1]/div/table/thead/tr[2]/td[4]/span/span/span/input")); }
        }
        public static IWebElement DriverSearchBar
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[1]/div/table/thead/tr[2]/td[5]/span/span/span/input")); }
        }
        public static IWebElement ShiftStatusSearchBar
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[1]/div/table/thead/tr[2]/td[6]/span/span/span/input")); }
        }
        public static IWebElement PaidOrVolunteerSearchBar
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[1]/div[2]/div/div[1]/div/table/thead/tr[2]/td[7]/span/span/span/input")); }
        }
    

    //ShiftModalView

    public static string ShiftDriver { get; set; }

        public static string PaidOrVolunteer { get; set; }

        public static string ShiftDateTime { get; set; }

        public static string NumberOfMovements { get; set; }

        public static string ShiftType { get; set; }

        public static string ShiftStatus { get; set; }


        public static IWebElement ModalShiftStatus
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"movementsStrip-1\"]/div[1]/div[2]/span")); }
        }
        public static IWebElement ModalView
        {
            get { return _driver.FindElement(By.XPath("//*[@id='shiftDetails']")); }
        }
        public static IWebElement ModalShiftName
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"shiftDetails\"]/div/div[1]/div[1]/div")); }
        }
        public static IWebElement ModalShiftDate
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"shiftDetails\"]/div/div[1]/div[1]/span[1]")); }
        }
        public static IWebElement ModalShiftTime
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"shiftDetails\"]/div/div[1]/div[1]/span[2]")); }
        }
        public static IWebElement ModalShiftType
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"shiftDetails\"]/div/div[1]/div[2]")); }
        }
        public static IWebElement ModalMovementId
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"movementsStrip-1\"]/div[1]/div[1]/div[1]/p")); }
        }
        public static IWebElement ModalServiceLevel
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"movementsStrip-1\"]/div[1]/div[1]/div[2]/p")); }
        }
        public static IWebElement ModalVehicleType
        {
            get { return _driver.FindElement(By.XPath("//*[@id='VehicleType']")); }
        }
        public static IWebElement ModalVehicle
        {
            get { return _driver.FindElement(By.XPath("//*[@id='Vehicle']")); }
        }
        public static IWebElement ModalVehicleEditButton
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"movementsStrip-1\"]/div[1]/div[3]/div/div[1]/button")); }
        }
        public static IWebElement ModalVehicleExitButton
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[29]/div[1]/div/a")); }
        }
        public static IWebElement ModalAssignVehicleDropDown
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"vehicleSelectModal\"]/span")); }
        }
        public static IWebElement ModalVehicleEditSaveButton
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"saveVehicleSelect\"]")); }
        }
        public static IWebElement ModalVehicleEditCloseButton
        {
            get { return _driver.FindElement(By.XPath("//*[@id=\"closeVehicleSelect\"]")); }
        }



        //Movement

        public static string MovementName => $"M-{TodaysDate.ToString("MMddyy")}_RCD_Test_Route_Baltimore_1200_Su-Sa";

        public static IWebElement MovementsGrid
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[2]/table/tbody")); }
        }

        public static IWebElement MovementSearchBar
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[2]/td[1]/span/span/span/input")); }
        }

        public static IWebElement MovementPickUpDate
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[2]/td[2]/span/span/span/input")); }
        }

        public static IWebElement MovementEndDate
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[2]/td[3]/span/span/span/input")); }
        }

        public static IWebElement MovementStops
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[2]/td[4]/span/span/span/input")); }
        }

        public static IWebElement MovementServiceLevel
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[2]/td[5]/span/span/span/input[1]")); }
        }

        public static IWebElement MovementStatus
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[2]/td[6]/span/span/span/input")); }
        }

        public static IWebElement MovementServiceLevelDropdown
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[2]/td[5]/span/span/span")); }
        }

        public static IWebElement AssignedToShiftName
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[2]/td[7]/span/span/span/input")); }
        }

        public static IWebElement MovementOrderButton
        {
            get { return _driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div[2]/div[2]/div/div[1]/div/table/thead/tr[1]/th[1]/span/span")); }
        }

    }
}
