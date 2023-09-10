using NUnit.Framework;
using UITesting.Pages;

namespace UITesting.TestSuites
{
    public class Overview : OverviewViewModel
    {
        [Test]
        //17477
        public static void VerifyShiftGridDisplay()
        {
           
            pgOverview.SetUpOverview();

            pgOverview.ValidateShiftName();

            pgOverview.ValidateShiftTypeDropDown();

            pgOverview.ValidateShiftCalendar();

            //pgOverview.ValidateShiftStatusColor();

        }

        [Test]
        //17476
        public static void VerifyShiftFromAndToDates()
        {
            pgOverview.SetUpOverview();

            pgOverview.ValidateShiftCalendar();

            pgOverview.ValidateShiftName();

        }

        [Test]
        //17534
        public static void VerifyShiftDateAndTimeColumn()
        {
            pgOverview.SetUpOverview();

            pgOverview.ValidateShiftDateAndTimeColumn();


        }

        [Test]
        //17478
        public static void VerifyShiftSortingAndSearchBars()
        {
            pgOverview.SetUpOverview();

            OrderButton.Click();

            pgOverview.ValidateShiftOrder("Ascending");

            OrderButton.Click();

            pgOverview.ValidateShiftOrder("Descending");

            pgOverview.ValidateShiftSearchBars();
        }

        [Test]
        //17536
        public static void VerifyMovementGridDisplay()
        {
            pgOverview.SetUpOverview();

            pgOverview.ValidateMovementName();

            pgOverview.ValidateMovementTypeDropDown();

            pgOverview.ValidateMovementCalendar();

            pgOverview.ValidateMovementStatusColor();

        }

        [Test]
        
        //17535
        public static void VerifyMovementFromAndToDates()
        {
            pgOverview.SetUpOverview();

            pgOverview.ValidateMovementCalendar();

            pgOverview.ValidateMovementName();

        }

        [Test]
        
        //17538
        public static void VerifyMovementDateAndTimeColumn()
        {
            pgOverview.SetUpOverview();

            pgOverview.ValidateMovementDateAndTimeColumn();


        }

        [Test]
        
        //17537
        public static void VerifyMovementSortingAndSearchBars()
        {
            pgOverview.SetUpOverview();

            MovementOrderButton.Click();

            pgOverview.ValidateMovementOrder("Ascending");

            MovementOrderButton.Click();

            pgOverview.ValidateMovementOrder("Descending");

            pgOverview.ValidateMovementSearchBars();
        }

        [Test]
        
        //17566
        public static void VerifyShiftViewHeadersAndMovementGrid()
        {
            pgOverview.SetUpOverview();

            pgOverview.GrabShiftData();

            pgOverview.OpenViewWindow();

            pgOverview.ValidateModalShiftNameAndDate();

            pgOverview.ValidateModalMovementCountAndShiftId();

            pgOverview.ValidateModalShiftStatus();

            pgOverview.ValidateModalMovementGrid();
        }

        [Test]
        
        //17865
        public static void VerifyModalShiftType ()
        {
            pgOverview.SetUpOverview();

            pgOverview.GrabShiftData();

            pgOverview.OpenViewWindow();

            pgOverview.ValidateModalMovementCountAndShiftId();
        }

        [Test]
        
        //1763
        public static void VerifyModalMovementGridValues()
        {
            pgOverview.SetUpOverview();

            pgOverview.GrabShiftData();

            pgOverview.OpenViewWindow();

            pgOverview.ValidateModalMovementGrid();

            pgOverview.ValidateModalDriverEdits();

        }


    }
}
