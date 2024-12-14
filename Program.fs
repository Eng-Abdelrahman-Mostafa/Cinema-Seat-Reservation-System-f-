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


let rows = 5
let cols = 8
let seatSize = 50
let seatLayout = Array2D.init rows cols (fun row col -> sprintf "%c%d" (char (row + int 'A')) (col + 1))
let reservedSeats = new HashSet<string>() //using Hash set why ??
//patern Matching Req
let getTicketFilePath timeSlot =
    match timeSlot with // like switch 
    | "02:00 PM" -> "tickets.txt"
    | "04:00 PM" -> "tickets1.txt"
    | "06:00 PM" -> "tickets2.txt"
    | _ -> "tickets.txt"  //Default
    

let loadReservedSeats timeSlot =
    let filePath = getTicketFilePath timeSlot
    if File.Exists(filePath) then
        File.ReadLines(filePath)
        |> Seq.iter (fun line ->
            let parts = line.Split(',')
            if parts.Length >= 3 then // checking valid data formate
                let seat = parts.[1].Split(':').[1].Trim() //trim remove any extra spaces
                reservedSeats.Add(seat) |> ignore
        )
        let saveTicketDetails ticketID seat customer ticketFilePath =  
    let ticketDetails = sprintf "Ticket ID: %s, Seat: %s, Customer: %s" ticketID seat customer
    File.AppendAllText(ticketFilePath, ticketDetails + Environment.NewLine)


let bookSeat (row: string) (col: string) (customer: string) =
    try
        let rowIndex = int row.[0] - int 'A'
        let colIndex = int col - 1
        if rowIndex < 0 || rowIndex >= rows || colIndex < 0 || colIndex >= cols then  // for checking that i ididnt breake the outline
            MessageBox.Show("Invalid seat coordinates!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        else
            let seat = seatLayout.[rowIndex, colIndex]
            if reservedSeats.Contains(seat) then
                MessageBox.Show(sprintf "Seat %s is already reserved!" seat, "Reservation Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
            else
                reservedSeats.Add(seat) |> ignore

                let ticketID = Guid.NewGuid().ToString() //making new iD

                let ticketFilePath = getTicketFilePath (cmbTimeSlot.SelectedItem :?> string)
                saveTicketDetails ticketID seat customer ticketFilePath

                for button in panel.Controls do  // after choosing index iterate over Arr to know where this index and make it red
                    match button with
                    | :? Button as btn when btn.Text = seat -> btn.BackColor <- Color.Red
                    | _ -> ()
                MessageBox.Show(sprintf "Seat %s has been reserved.\nTicket ID: %s" seat ticketID, "Reservation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
    with
    | _ -> MessageBox.Show("Invalid input! Please enter a valid row and column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

btnBook.Click.Add(fun _ ->
    let row = txtRow.Text.Trim().ToUpper()
    let col = txtCol.Text.Trim()
    let customer = txtCustomer.Text.Trim()


    let selectedTimeSlot = cmbTimeSlot.SelectedItem :?> string


    let ticketFilePath = getTicketFilePath selectedTimeSlot

    if row = "" || col = "" || customer = "" then   // cehcking That all data had been writen
        MessageBox.Show("All fields are required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
    else
        bookSeat row col customer
)
