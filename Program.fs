open System
open System.IO
open System.Drawing
open System.Windows.Forms
open System.Collections.Generic


let form = new Form(Text = "Cinema Seat Reservation", Size = Size(800, 600))


let panel = new Panel(Dock = DockStyle.Top, BackColor = Color.FromArgb(0, 109, 102), Height = 300)
form.Controls.Add(panel)
form.BackColor <- Color.FromArgb(0, 109, 102)

//Labels
let lblRow = new Label(Text = "Row (A-E):", Location = Point(10, 320))
let lblCol = new Label(Text = "Column (1-8):", Location = Point(10, 350))
let txtRow = new TextBox(Location = Point(120, 320), Width = 300)
let txtCol = new TextBox(Location = Point(120, 350), Width = 300)
let lblCustomer = new Label(Text = "Customer Name:", Location = Point(10, 380))
let txtCustomer = new TextBox(Location = Point(120, 380), Width = 150)
let lblTime = new Label(Text = "Time Slot:", Location = Point(10, 410))
let cmbTimeSlot = new ComboBox(Location = Point(120, 410), Width = 150)
let btnBook = new Button(Text = "Book Seat", Location = Point(50, 450), Width = 100)
let btnLoadReservedSeats = new Button(Text = "Load Reserved Seats", Location = Point(200, 450), Width = 150)

//checkList define by Array
let timeSlots = [| "02:00 PM"; "04:00 PM"; "06:00 PM" |]
cmbTimeSlot.Items.AddRange(timeSlots |> Array.map box) // string-> obj in checkList 
// passing all textBooxes 
form.Controls.AddRange([| lblRow; lblCol; txtRow; txtCol; lblCustomer; txtCustomer; lblTime; cmbTimeSlot; btnBook; btnLoadReservedSeats |])
