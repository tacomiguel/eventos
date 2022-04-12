Imports MySql.Data.MySqlClient

Public Class c_eventos
    Inherits Office2007Form
    Private nroOperacion As Integer


#Region "Private variables"

    Private _UserStyleSet As Boolean

#End Region
    'Private defUsers As String() = New String() {"SALON DE EVENTOS", "CUPULA", "ESPIGON", "SALON VIP", "BAR PRINCIPAL"}

    Sub New()

        BarUtilities.UseTextRenderer = True
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        ' Set our Calendar Model

        'AddSampleAppointments()
        'AddHolidaySchedule()
        cargarsalones(1)
        ' cargareventos()
        ' Hook onto the following events so we can be receive
        ' appointment StartTime and Reminder notifications

        AddHandler CalendarView1.AppointmentReminder, AddressOf AppointmentReminder
        AddHandler CalendarView1.AppointmentStartTimeReached, AddressOf AppointmentStartTimeReached

        ' Uncomment out the following lines of code it you want to
        ' see appointment move and resize events as they are processed

        ' AddHandler CalendarView1.CalendarModel.AppointmentAdded, AddressOf ModelAppointmentAdded
        ' AddHandler CalendarView1.CalendarModel.AppointmentRemoved, AddressOf ModelAppointmentRemoved
        ' AddHandler CalendarView1.AppointmentViewChanged, AddressOf AppointmentViewChanged

    End Sub

#Region "Handled events"

    ''' <summary>
    ''' Event sent when an appointment is added to the Model
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ModelAppointmentAdded(ByVal sender As Object, ByVal e As AppointmentEventArgs)
        Console.WriteLine("{0} New Appointment Added. Appointment Subject: {1}", _
            DateTime.Now, e.Appointment.Subject)
    End Sub

    ''' <summary>
    ''' Event sent when an appointment is removed from the Model
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ModelAppointmentRemoved(ByVal sender As Object, ByVal e As AppointmentEventArgs)
        Console.WriteLine("{0} Appointment Removed. Appointment Subject: {1}", _
            DateTime.Now, e.Appointment.Subject)
    End Sub

    ''' <summary>
    ''' Event sent when an appointment is either moved or resized
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AppointmentViewChanged(ByVal sender As Object, ByVal e As AppointmentViewChangedEventArgs)
        Dim view As AppointmentView = TryCast(e.CalendarItem, AppointmentView)

        If view IsNot Nothing Then
            Dim sOperation As String

            If e.eViewOperation = eViewOperation.AppointmentMove Then
                sOperation = "Moved"
            Else
                sOperation = "Resized"
            End If

            Console.WriteLine("{0} Appointment {1}. Appointment Subject: {2}", _
                DateTime.Now, sOperation, view.Appointment.Subject)
        End If
    End Sub

    ''' <summary>
    ''' Event sent when an appointment reminder is reached
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AppointmentReminder(ByVal sender As Object, ByVal e As ReminderEventArgs)
        Dim s As String = String.Format("Appointment Reminder reached for Appointment: '{0}'", _
            e.Reminder.Appointment.Subject)

        MessageBox.Show(s)
    End Sub

    ''' <summary>
    ''' Event sent when an appointment StartTime is reached
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AppointmentStartTimeReached(ByVal sender As Object, ByVal e As AppointmentEventArgs)
        Dim s As String = String.Format("Appointment StartTime Reached for Appointment: '{0}'", _
            e.Appointment.Subject)

        MessageBox.Show(s)
    End Sub
#End Region

#Region "AddSampleAppointments"

    ''' <summary>
    ''' Adds some example appointments to the Calendar Model
    ''' </summary>
    Private Sub AddSampleAppointments()
        ' Recurring appointment

        Dim appointment As New Appointment()

        appointment.Subject = "Recurring Appointment every 3rd week day"
        appointment.Description = "Custom description for this appointment"
        appointment.Tooltip = "This is a Custom tooltip for this appointment"
        appointment.CategoryColor = appointment.CategoryOrange

        appointment.StartTime = DateTime.Today.AddHours(11.25)
        appointment.EndTime = appointment.StartTime.AddHours(1)

        appointment.Recurrence = New AppointmentRecurrence()

        ' Set recurrence type to Daily

        appointment.Recurrence.RecurrenceType = eRecurrencePatternType.Daily
        appointment.Recurrence.Daily.RepeatOnDaysOfWeek = eDailyRecurrenceRepeat.All
        appointment.Recurrence.Daily.RepeatInterval = 10
        appointment.Recurrence.RangeLimitType = eRecurrenceRangeLimitType.RangeEndDate
        appointment.Recurrence.RangeEndDate = DateTime.Today.AddDays(120)

        ' Add appointment to the model

        CalendarView1.CalendarModel.Appointments.Add(appointment)

        Dim StartDate As DateTime = DateTime.Today.AddHours(8)

        ' Multi-day appointment #1

        'AddAppointment("#1 Multiple days Appointment", StartDate, StartDate.AddDays(4), _
        '    appointment.CategoryBlue, appointment.TimerMarkerDefault)

        '' Multi-day appointment #2

        'AddAppointment("#2 Multiple days Appointment", StartDate.AddDays(2), StartDate.AddDays(14.5), _
        '    appointment.CategoryGreen, appointment.TimerMarkerDefault)

        '' Create some new appointments and add them to the model

        'Dim ap As Appointment = AddAppointment("Busy Appointment", StartDate, StartDate.AddMinutes(180), _
        '    appointment.CategoryYellow, appointment.TimerMarkerBusy)

        ' Make sure this appointment is visible

        'CalendarView1.EnsureVisible(ap)

        'AddAppointment("Out Of Office Appointment", StartDate.AddHours(1), StartDate.AddHours(2), _
        '    appointment.CategoryBlue, appointment.TimerMarkerOutOfOffice)

        'StartDate = StartDate.AddDays(1)

        'AddAppointment("Free Appointment", StartDate.AddHours(2), StartDate.AddMinutes(170), _
        '    appointment.CategoryRed, appointment.TimerMarkerFree)

        'AddAppointment("Tentative Appointment", StartDate.AddHours(4), StartDate.AddHours(6), _
        '    appointment.CategoryDefault, appointment.TimerMarkerTentative)

        'StartDate = StartDate.AddDays(1)

        'AddAppointment("Default Appointment", StartDate.AddHours(1.5), StartDate.AddHours(4.25), _
        '    appointment.CategoryPurple, appointment.TimerMarkerDefault)

        'StartDate = StartDate.AddDays(1)

        'AddAppointment("New Appointment", StartDate.AddHours(11), StartDate.AddHours(11).AddMinutes(30), _
        '    appointment.CategoryYellow, appointment.TimerMarkerDefault)

    End Sub

#Region "AddAppointment"

    ''' <summary>
    ''' Adds the specified appointment to the model
    ''' </summary>
    ''' <param name="s">Appointment subject</param>
    ''' <param name="startTime">Appointment start time</param>
    ''' <param name="endTime">Appointment end time</param>
    ''' <param name="color">Appointment color</param>
    ''' <param name="marker">Appointment marker</param>
    Private Function AddAppointment(ByVal id As Integer, ByVal s As String, ByVal o As String, ByVal startTime As DateTime, ByVal endTime As DateTime, ByVal color As String, ByVal salon As String, ByVal marker As String) As Appointment
        Dim appointment As New Appointment()

        appointment.StartTime = startTime
        appointment.EndTime = endTime
        appointment.Id = id
        appointment.OwnerKey = salon

        appointment.Subject = s
        appointment.Description = o
        appointment.CategoryColor = color
        appointment.TimeMarkedAs = marker

        appointment.Tooltip = "This is a Custom tooltip for this appointment"




        CalendarView1.CalendarModel.Appointments.Add(appointment)

        Return (appointment)
    End Function

#End Region

#End Region

#Region "AddHolidaySchedule"

    ''' <summary>
    ''' Adds our Holiday schedule, starting January 1, 2010
    ''' and continuing for the next 10 years
    ''' </summary>
    Private Sub AddHolidaySchedule()
        AddHoliday("New Years Day", 1, 1)
        AddHoliday("Martin Luther King Jr Day", 1, eRelativeDayInMonth.Third, DayOfWeek.Monday)
        AddHoliday("Groundhog Day", 2, 2)
        AddHoliday("Valentines Day", 2, 14)
        AddHoliday("Presidents Day", 2, eRelativeDayInMonth.Third, DayOfWeek.Monday)
        AddHoliday("St. Patricks Day", 3, 17)
        AddHoliday("April Fools Day", 4, 1)
        AddHoliday("Earth Day", 4, 22)
        AddHoliday("Cinco de Mayo", 5, 5)
        AddHoliday("Mothers Day", 5, eRelativeDayInMonth.Second, DayOfWeek.Saturday)
        AddHoliday("Memorial Day", 5, eRelativeDayInMonth.Last, DayOfWeek.Monday)
        AddHoliday("Flag Day", 6, 14)
        AddHoliday("Fathers Day", 6, eRelativeDayInMonth.Third, DayOfWeek.Sunday)
        AddHoliday("Independence Day", 7, 4)
        AddHoliday("Labor Day", 9, eRelativeDayInMonth.First, DayOfWeek.Monday)
        AddHoliday("Columbus Day", 10, eRelativeDayInMonth.Second, DayOfWeek.Monday)
        AddHoliday("Halloween", 10, 31)
        AddHoliday("Veterans Day", 11, 11)
        AddHoliday("Thanksgiving", 11, eRelativeDayInMonth.Fourth, DayOfWeek.Thursday)
        AddHoliday("Christmas", 12, 25)
        AddHoliday("New Years Eve", 12, 31)

        ' Add Easter

        AddEaster()
    End Sub

#Region "AddHoliday"

    ''' <summary>
    ''' Adds the given "Absolute" Holiday to the calendar
    ''' </summary>
    ''' <param name="title">Holiday title</param>
    ''' <param name="month">Month</param>
    ''' <param name="day">Day</param>
    Private Sub AddHoliday(ByVal title As String, ByVal month As Integer, ByVal day As Integer)
        Dim appointment As Appointment = NewHoliday(title, New DateTime(2010, month, day))

        appointment.Recurrence.Yearly.RepeatOnMonth = month
        appointment.Recurrence.Yearly.RepeatOnDayOfMonth = day

        ' Add appointment to the model

        CalendarView1.CalendarModel.Appointments.Add(appointment)
    End Sub

    ''' <summary>
    ''' Adds the given "Relative" Holiday to the calendar
    ''' </summary>
    ''' <param name="title">Holiday title</param>
    ''' <param name="month">Month</param>
    ''' <param name="relDim">Day in Month</param>
    ''' <param name="dow">Day of Week</param>
    Private Sub AddHoliday(ByVal title As String, ByVal month As Integer, ByVal relDim As eRelativeDayInMonth, ByVal dow As DayOfWeek)
        Dim appointment As Appointment = NewHoliday(title, RelativeDate(month, relDim, dow))

        appointment.Recurrence.Yearly.RepeatOnMonth = month
        appointment.Recurrence.Yearly.RelativeDayOfWeek = dow
        appointment.Recurrence.Yearly.RepeatOnRelativeDayInMonth = relDim
        appointment.Recurrence.Yearly.RepeatOnDayOfMonth = 0

        ' Add appointment to the model

        CalendarView1.CalendarModel.Appointments.Add(appointment)
    End Sub

#End Region

#Region "RelativeDate"

    ''' <summary>
    ''' Calculates the date from the given
    ''' relative day in the month and day of the week
    ''' </summary>
    ''' <param name="month">Month</param>
    ''' <param name="relDim">Relative Day in Month</param>
    ''' <param name="dow">Day of Week</param>
    ''' <returns>Date</returns>
    Private Function RelativeDate(ByVal month As Integer, ByVal relDim As eRelativeDayInMonth, ByVal dow As DayOfWeek) As DateTime
        Dim relDate As New DateTime(2010, month, 1)

        While relDate.DayOfWeek <> dow
            relDate = relDate.AddDays(1)
        End While

        Select Case relDim
            Case eRelativeDayInMonth.First
                Return (relDate)

            Case eRelativeDayInMonth.Second
                Return (relDate.AddDays(7))

            Case eRelativeDayInMonth.Third
                Return (relDate.AddDays(14))

            Case eRelativeDayInMonth.Fourth
                Return (relDate.AddDays(21))
        End Select

        Return (relDate)
    End Function

#End Region

#Region "NewHoliday"

    ''' <summary>
    ''' Allocates a new Holiday appointment
    ''' </summary>
    ''' <param name="title">Holiday title</param>
    ''' <param name="startTime">Start time</param>
    ''' <returns>Allocated appointment</returns>
    Private Function NewHoliday(ByVal title As String, ByVal startTime As DateTime) As Appointment
        ' Allocate a new Appointment

        Dim appointment As New Appointment()

        appointment.Subject = title
        appointment.Tooltip = title
        appointment.CategoryColor = appointment.CategoryRed
        appointment.TimeMarkedAs = appointment.TimerMarkerFree
        appointment.Locked = True

        appointment.StartTime = startTime
        appointment.EndTime = startTime.AddDays(1)

        ' Allocate a new AppointmentRecurrence

        appointment.Recurrence = New AppointmentRecurrence()

        appointment.Recurrence.RecurrenceType = eRecurrencePatternType.Yearly
        appointment.Recurrence.RangeLimitType = eRecurrenceRangeLimitType.RangeEndDate
        appointment.Recurrence.RangeEndDate = DateTime.Today.AddYears(10)

        Return (appointment)
    End Function

#End Region

#Region "AddEaster"

    ''' <summary>
    ''' Add Easter appointments for the next 10 years
    ''' </summary>
    Private Sub AddEaster()
        ' Add 10 years worth of dates

        For i As Integer = 2010 To 2019
            Dim [date] As DateTime = EasterDate(i)

            ' Add Easter

            Dim appointment As New Appointment()

            appointment.StartTime = [date]
            appointment.EndTime = appointment.StartTime.AddDays(1)

            appointment.Subject = "Easter"
            appointment.Tooltip = appointment.Subject
            appointment.CategoryColor = appointment.CategoryRed
            appointment.TimeMarkedAs = appointment.TimerMarkerFree
            appointment.Locked = True

            CalendarView1.CalendarModel.Appointments.Add(appointment)

            ' Add Good Friday

            appointment = New Appointment()

            appointment.StartTime = [date].AddDays(-2)
            appointment.EndTime = appointment.StartTime.AddDays(1)

            appointment.Subject = "Good Friday"
            appointment.Tooltip = appointment.Subject
            appointment.OwnerKey = "Fred"
            appointment.CategoryColor = appointment.CategoryRed
            appointment.TimeMarkedAs = appointment.TimerMarkerFree
            appointment.Locked = True

            CalendarView1.CalendarModel.Appointments.Add(appointment)
        Next
    End Sub

#Region "EasterDate"

    ''' <summary>
    ''' Gets the Date for Easter for the given year
    ''' </summary>
    ''' <param name="year">Year</param>
    ''' <returns>Easter date</returns>
    Private Function EasterDate(ByVal year As Integer) As DateTime

        Dim g As Integer = year Mod 19
        Dim c As Integer = year \ 100
        Dim h As Integer = (c - (c \ 4) - ((8 * c + 13) \ 25) + 19 * g + 15) Mod 30
        Dim i As Integer = h - (h \ 28) * (1 - (h \ 28) * (29 \ (h + 1)) * ((21 - g) \ 11))

        Dim day As Integer = i - ((year + (year \ 4) + i + 2 - c + (c \ 4)) Mod 7) + 28
        Dim month As Integer = 3

        If day > 31 Then
            month = month + 1
            day = day - 31
        End If

        Return (New DateTime(year, month, day))

    End Function

#End Region

#End Region

#End Region

#Region "View Change"

    ''' <summary>
    ''' Day view selection
    ''' </summary>
    ''' <param name="sender">PopupItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ButtonDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDay.Click
        CalendarView1.SelectedView = eCalendarView.Day
    End Sub

    ''' <summary>
    ''' Week view selection
    ''' </summary>
    ''' <param name="sender">PopupItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ButtonWeek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonWeek.Click
        CalendarView1.SelectedView = eCalendarView.Week
    End Sub

    ''' <summary>
    ''' Month view selection
    ''' </summary>
    ''' <param name="sender">PopupItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ButtonMonth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMonth.Click
        CalendarView1.SelectedView = eCalendarView.Month
    End Sub

    ''' <summary>
    ''' Year view selection
    ''' </summary>
    ''' <param name="sender">PopupItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub btnYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYear.Click
        CalendarView1.SelectedView = eCalendarView.Year
    End Sub

    ''' <summary>
    ''' TimeLine view selection
    ''' </summary>
    ''' <param name="sender">PopupItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub ButtonTimeLine_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTimeLine.Click
        CalendarView1.SelectedView = eCalendarView.TimeLine
    End Sub

#End Region

#Region "calendarView1_ItemDoubleClick"

    ''' <summary>
    ''' Handles Appointment View double clicks
    ''' </summary>
    ''' <param name="sender">AppointmentView</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub calendarView1_ItemDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles CalendarView1.ItemDoubleClick
        Dim item As AppointmentView = TryCast(sender, AppointmentView)

        If item IsNot Nothing Then
            Dim ap As Appointment = item.Appointment
            Dim CategoryColor As String = ap.CategoryColor
            Dim TimeMarkedAs As String = ap.TimeMarkedAs

            If String.IsNullOrEmpty(CategoryColor) Then
                CategoryColor = "Default"
            End If

            If String.IsNullOrEmpty(TimeMarkedAs) Then
                TimeMarkedAs = "Default"
            End If

            cargaForm_P_eventos(ap.Id, ap.StartTime, ap.EndTime, ap.OwnerKey)


            'Dim s As String = String.Format("Subject: {0}" & vbLf & "Description: {1}" & vbLf & _
            '    "ToolTip: {2}" & vbLf & vbLf & "StartTime: {3}" & vbLf & "EndTime: {4}" & vbLf & vbLf & _
            '    "CategoryColor: {5}" & vbLf & "TimeMarkedAs: {6}" _
            '    , ap.Subject, ap.Description, ap.Tooltip, ap.StartTime, ap.EndTime, ap.Id, ap.AutoId)

            'MessageBox.Show(s)

        End If
    End Sub

    Sub cargaForm_P_eventos(ByVal operacion As Integer, ByVal fec_desde As DateTime, ByVal fec_hasta As DateTime, ByVal salon As String)
        Me.Close()
        Dim IsFormCargado As Boolean = False
        Dim mForm As Form
        For Each mForm In principal.MdiChildren
            If mForm.Name = "p_eventos" Then
                If mForm.WindowState = FormWindowState.Minimized Then
                    mForm.WindowState = FormWindowState.Normal
                Else
                    mForm.BringToFront()
                End If
                IsFormCargado = True
                Exit For
            End If
        Next
        mForm = Nothing
        If IsFormCargado = False Then
            'Dim mformIngresos As New p_eventos
            'mformIngresos.MdiParent = principal
            p_eventos.MdiParent = principal
            ''mformIngresos.datosParaEdicion(lproveedor, ldocumento, lserdoc, lnrodoc)
            'mformIngresos.Show()
            p_eventos.Show()
            p_eventos.cargaeventos(operacion, fec_desde, fec_hasta, salon, False)
            'mformIngresos.Refresh()
            ''mformIngresos.dataDetalle.Focus()
        End If
        Me.Close()
    End Sub
    Sub cargarsalones(ByVal tip_salon As Integer)
        Dim dbConex As MySqlConnection = Conexion.obtenerConexion()
        Dim com As New MySqlCommand("select dsc_recurso from tipo_recurso where cod_tabla='tip_salon' and n_aux=" & tip_salon, dbConex)
        Dim dr As MySqlDataReader
        Dim i As Integer
        Dim defUsers(300) As String

        dr = com.ExecuteReader
        If dr.HasRows = True Then
            While dr.Read
                defUsers(i) = dr("dsc_recurso")
                i = i + 1
            End While
        End If
        CalendarView1.CalendarModel = New CalendarModel()
        CalendarView1.DisplayedOwners.AddRange(defUsers)
    End Sub
    Sub cargarsalones_filtro(ByVal dsc_salon As String)
        Dim dbConex As MySqlConnection = Conexion.obtenerConexion()
        Dim com As New MySqlCommand("select dsc_recurso from tipo_recurso where cod_tabla='tip_salon' and dsc_recurso like '%" & dsc_salon & "%'", dbConex)
        Dim dr As MySqlDataReader
        Dim i As Integer
        Dim defUsers(300) As String

        dr = com.ExecuteReader
        If dr.HasRows = True Then
            While dr.Read
                defUsers(i) = dr("dsc_recurso")
                i = i + 1
            End While
        End If
        CalendarView1.CalendarModel = New CalendarModel()
        CalendarView1.DisplayedOwners.AddRange(defUsers)
    End Sub

    Sub cargareventos()
        ' And add some sample appointments

        Dim dbConex As MySqlConnection = Conexion.obtenerConexion()
        Dim StartDate, EndDate As DateTime
        Dim evento, obs, cod_salon, salon, cod_estado, color, categoria As String
        Dim ID As Integer
        Dim com As New MySqlCommand("select operacion,fec_ini,fec_fin,nom_evento,obs,cod_salon,cod_estado from reservas ", dbConex)
        Dim dr As MySqlDataReader
        dr = com.ExecuteReader
        If dr.HasRows = True Then
            While dr.Read
                StartDate = dr("fec_ini")
                EndDate = dr("fec_fin")
                evento = dr("nom_evento")
                obs = dr("obs")
                ID = dr("operacion")
                cod_salon = dr("cod_salon")
                salon = consultas.devuelve_nomrecurso(cod_salon)
                cod_estado = dr("cod_estado")
                color = consultas.devuelve_caux(cod_estado)

                Select Case color
                    Case "Red"
                        categoria = Appointment.CategoryRed
                    Case "Blue"
                        categoria = Appointment.CategoryBlue
                    Case "Yellow"
                        categoria = Appointment.CategoryYellow
                    Case "Green"
                        categoria = Appointment.CategoryGreen
                    Case Else
                        categoria = Appointment.CategoryOrange
                End Select
                AddAppointment(ID, "Evento :" & evento, "Observacion:" & obs, StartDate, EndDate, categoria, salon, Appointment.TimerMarkerFree)
            End While
        End If
        dr.Close()
    End Sub

    Sub eliminaSalones()
        Dim dbConex As MySqlConnection = Conexion.obtenerConexion()
        Dim com As New MySqlCommand("select dsc_recurso from tipo_recurso where cod_tabla='tip_salon' ", dbConex)
        Dim dr As MySqlDataReader
        Dim i As Integer
        Dim defUsers(300) As String

        dr = com.ExecuteReader
        If dr.HasRows = True Then
            While dr.Read
                defUsers(i) = dr("dsc_recurso")

                CalendarView1.DisplayedOwners.Remove(defUsers(i))
                i = i + 1
            End While
        End If
        'CalendarView1.CalendarModel = New CalendarModel()

    End Sub
    Sub mivariable(ByVal operacion As Integer)
        nroOperacion = operacion
    End Sub

    Sub cargarMievento()
        ' And add some sample appointments

        Dim dbConex As MySqlConnection = Conexion.obtenerConexion()
        Dim StartDate, EndDate As DateTime
        Dim evento, obs As String
        Dim ID As Integer
        Dim com As New MySqlCommand("select * from reservas where operacion= " & nroOperacion, dbConex)
        Dim dr As MySqlDataReader
        dr = com.ExecuteReader
        If dr.HasRows = True Then
            While dr.Read
                StartDate = dr("fec_ini")
                EndDate = dr("fec_fin")
                evento = dr("nom_evento")
                obs = dr("obs")
                ID = dr("operacion")

                AddAppointment(ID, "Evento :" & evento, "Observacion:" & obs, StartDate, EndDate, _
            Appointment.CategoryRed, "", Appointment.TimerMarkerFree)
            End While
        End If
        dr.Close()
    End Sub

#End Region

#Region "calendarView1_MouseUp"

    ''' <summary>
    ''' Handles CalendarView MouseUp events
    ''' </summary>
    ''' <param name="sender">Varied sender</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub calendarView1_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles CalendarView1.MouseUp
        ' Process Right MouseUp event in order to
        ' present view specific ContextMenu

        If e.Button = MouseButtons.Right Then
            ' Main Calendar View hit

            If TypeOf sender Is BaseView Then
                BaseViewMouseUp(sender, e)

                ' AppointmentView hit

            ElseIf TypeOf sender Is AppointmentView Then
                AppointmentViewMouseUp(sender, e)

                ' AllDayPanel hit

            ElseIf TypeOf sender Is AllDayPanel Then
                AllDayPanelMouseUp(sender, e)

                ' TimeRulerPanel hit

            ElseIf TypeOf sender Is TimeRulerPanel Then
                TimeRulerPanelMouseUp(sender, e)

                ' TimeLineHeaderPanel

            ElseIf TypeOf sender Is TimeLineHeaderPanel Then
                TimeLineHeaderPanelMouseUp(sender, e)
            End If
        End If
    End Sub

#Region "BaseViewMouseUp"

    ''' <summary>
    ''' Handles Day, Week, and Month View MouseUp events
    ''' </summary>
    ''' <param name="sender">BaseView</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub BaseViewMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim view As BaseView = TryCast(sender, BaseView)
        Dim area As eViewArea = view.GetViewAreaFromPoint(e.Location)

        If area = eViewArea.InContent Then
            InContentMouseUp(view, e)

        ElseIf area = eViewArea.InDayOfWeekHeader Then
            InDayOfWeekHeaderMouseUp(view, e)

        ElseIf area = eViewArea.InMonthHeader Then
            InMonthHeaderMouseUp(view, e)

        ElseIf area = eViewArea.InSideBarPanel Then
            InSideBarMouseUp(view, e)

        ElseIf area = eViewArea.InCondensedView Then
            InCondensedViewMouseUp(e)
        End If
    End Sub

#Region "InContentMouseUp"

    ''' <summary>
    ''' Handles BaseView InContent MouseUp events
    ''' </summary>
    ''' <param name="view">BaseView</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub InContentMouseUp(ByVal view As BaseView, ByVal e As MouseEventArgs)
        ' Get the DateSelection start and end
        ' from the current mouse location

        Dim startDate As DateTime, endDate As DateTime

        If CalendarView1.GetDateSelectionFromPoint(e.Location, startDate, endDate) = True Then
            ' If this date already falls outside the currently
            ' selected range (DateSelectionStart and DateSelectionEnd)
            ' then select the new range

            If IsDateSelected(startDate, endDate) = False Then
                CalendarView1.DateSelectionStart = startDate
                CalendarView1.DateSelectionEnd = endDate
            End If
        End If

        ' Remove any previously added view specific items

        For i As Integer = InContentContextMenu.SubItems.Count - 1 To 1 Step -1
            InContentContextMenu.SubItems.RemoveAt(i)
        Next

        ' If this is a TimeLine view, then add a couple
        ' of extra items

        If TypeOf view Is TimeLineView Then
            Dim bi As New ButtonItem()

            ' Page Navigator

            If CalendarView1.TimeLineShowPageNavigation = True Then
                bi.Text = "Hide Page Navigator"
            Else
                bi.Text = "Show Page Navigator"
            End If

            bi.BeginGroup = True

            AddHandler bi.Click, AddressOf bi_PageNavigatorClick

            InContentContextMenu.SubItems.Add(bi)

            ' Condensed Visibility

            If CalendarView1.TimeLineCondensedViewVisibility = eCondensedViewVisibility.Hidden Then
                bi = New ButtonItem("", "Show Condensed View")
                AddHandler bi.Click, AddressOf bi_CondensedClick

                InContentContextMenu.SubItems.Add(bi)
            End If
        End If

        ShowContextMenu(InContentContextMenu)
    End Sub

#Region "bi_CondensedClick"

    ''' <summary>
    ''' Handles InContentContextMenu Condensed selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub bi_CondensedClick(ByVal sender As Object, ByVal e As EventArgs)
        CalendarView1.TimeLineCondensedViewVisibility = eCondensedViewVisibility.AllResources
    End Sub

#End Region

#Region "bi_PageNavigatorClick"

    ''' <summary>
    ''' Handles InContentContextMenu PageNavigator selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub bi_PageNavigatorClick(ByVal sender As Object, ByVal e As EventArgs)
        CalendarView1.TimeLineShowPageNavigation = (CalendarView1.TimeLineShowPageNavigation = False)
    End Sub

#End Region

#Region "IsDateSelected"

    ''' <summary>
    ''' Determines if the given date range is within the currently selected
    ''' CalendarView selection range (DateSelectionStart to DateSelectionEnd)
    ''' </summary>
    ''' <param name="startDate">Start date range</param>
    ''' <param name="endDate">End date range</param>
    ''' <returns>True if selected</returns>
    Private Function IsDateSelected(ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
        If CalendarView1.DateSelectionStart.HasValue AndAlso CalendarView1.DateSelectionEnd.HasValue Then
            If CalendarView1.DateSelectionStart.Value <= startDate AndAlso CalendarView1.DateSelectionEnd.Value >= endDate Then
                Return (True)
            End If
        End If

        Return (False)
    End Function

#End Region

#Region "miAdd_Click"

    ''' <summary>
    ''' Handles BaseView "Add Appointment" 
    ''' ContextMenu selections
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles InContentAddAppContextItem.Click
        Dim startDate As DateTime = CalendarView1.DateSelectionStart.GetValueOrDefault()
        Dim endDate As DateTime = CalendarView1.DateSelectionEnd.GetValueOrDefault()

        AddNewAppointment(startDate, endDate)
    End Sub

    ''' <summary>
    ''' Adds a new appointment at the user selected time
    ''' </summary>
    Private Function AddNewAppointment(ByVal startDate As DateTime, ByVal endDate As DateTime) As Appointment
        ' Create new appointment and add it to the model
        ' Appointment will show up in the view automatically

        Dim appointment As New Appointment()

        appointment.StartTime = startDate
        appointment.EndTime = endDate
        appointment.OwnerKey = CalendarView1.SelectedOwner

        appointment.Subject = "Nuevo Evento!"

        appointment.Description = "Observacion"
        appointment.Tooltip = ""

        ' Add appointment to the model

        CalendarView1.CalendarModel.Appointments.Add(appointment)

        Return (appointment)
    End Function

#End Region

#End Region

#Region "InDayOfWeekHeaderMouseUp"

    ''' <summary>
    ''' Handles BaseView InDayOfWeekHeader MouseUp events.
    ''' </summary>
    ''' <param name="view">BaseView</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub InDayOfWeekHeaderMouseUp(ByVal view As BaseView, ByVal e As MouseEventArgs)
        If TypeOf view Is MonthView Then
            Dim mv As MonthView = TryCast(view, MonthView)

            ' The View is a month view, so let the user
            ' hide or show the SideBar panel

            If mv.IsSideBarVisible = True Then
                InHeaderHideSideBarContextItem.Visible = True
                InHeaderShowSideBarContextItem.Visible = False
            Else
                InHeaderHideSideBarContextItem.Visible = False
                InHeaderShowSideBarContextItem.Visible = True
            End If
        Else
            InHeaderHideSideBarContextItem.Visible = False
            InHeaderShowSideBarContextItem.Visible = False
        End If

        InHeaderContextMenu.Tag = view

        ShowContextMenu(InHeaderContextMenu)
    End Sub

#Region "miCalendarColor_Click"

    ''' <summary>
    ''' Handles selection of a ContextMenu color item
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miCalendarColor_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim mi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As BaseView = TryCast(mi.Parent.Tag, BaseView)

        If view IsNot Nothing Then
            view.CalendarColor = DirectCast([Enum].Parse(GetType(eCalendarColor), mi.Text), eCalendarColor)
        End If
    End Sub

#End Region

#Region "SideBar show/hide"

    ''' <summary>
    ''' Handles ContextMenu "Show" for the SideBar panel
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miShowSideBar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As MonthView = TryCast(bi.Parent.Tag, MonthView)

        If view IsNot Nothing Then
            view.IsSideBarVisible = True
        End If
    End Sub

    ''' <summary>
    ''' Handles ContextMenu "Hide" for the SideBar panel
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miHideSideBar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As MonthView = TryCast(bi.Parent.Tag, MonthView)

        If view IsNot Nothing Then
            view.IsSideBarVisible = False
        End If
    End Sub


#End Region

#End Region

#Region "InMonthHeaderMouseUp"

    ''' <summary>
    ''' Handles BaseView InMonthHeader MouseUp events.
    ''' </summary>
    ''' <param name="view">BaseView</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub InMonthHeaderMouseUp(ByVal view As BaseView, ByVal e As MouseEventArgs)
        If TypeOf view Is YearView Then
            Dim yv As YearView = TryCast(view, YearView)

            ' The View is a Year view, so let the user
            ' hide or show the Gridlines

            InMonthHeaderHideGridLinesContextItem.Visible = CalendarView1.YearViewShowGridLines
            InMonthHeaderShowGridLinesContextItem.Visible = Not CalendarView1.YearViewShowGridLines
        End If

        InMonthHeaderContextMenu.Tag = view

        ShowContextMenu(InMonthHeaderContextMenu)
    End Sub

#Region "Gridlines show/hide"

    ''' <summary>
    ''' Handles ContextMenu "Show GridLines" for the Year View
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miShowGridLines_Click(ByVal sender As Object, ByVal e As EventArgs) Handles InMonthHeaderShowGridLinesContextItem.Click
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As YearView = TryCast(bi.Parent.Tag, YearView)

        If view IsNot Nothing Then
            CalendarView1.YearViewShowGridLines = True
        End If
    End Sub

    ''' <summary>
    ''' Handles ContextMenu "Hide GridLines" for the Year View
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miHideGridLines_Click(ByVal sender As Object, ByVal e As EventArgs) Handles InMonthHeaderHideGridLinesContextItem.Click
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As YearView = TryCast(bi.Parent.Tag, YearView)

        If view IsNot Nothing Then
            CalendarView1.YearViewShowGridLines = False
        End If
    End Sub

#End Region

#Region "miCycleHighlight_Click"

    ''' <summary>
    ''' Handles ContextMenu "Cycle Highlight" for the Year View.  This
    ''' routine cycles through all the possible Day Link Highlight values
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miCycleHighlight_Click(ByVal sender As Object, ByVal e As EventArgs) Handles InMonthHeaderCycleStyle.Click
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As YearView = TryCast(bi.Parent.Tag, YearView)

        If view IsNot Nothing Then
            If CalendarView1.YearViewAppointmentLinkStyle = eYearViewLinkStyle.None Then
                If _UserStyleSet = False Then
                    _UserStyleSet = True

                    AddHandler CalendarView1.YearViewDrawDayBackground, AddressOf YearViewDrawDayBackground

                    CalendarView1.Refresh()
                Else
                    _UserStyleSet = False

                    RemoveHandler CalendarView1.YearViewDrawDayBackground, AddressOf YearViewDrawDayBackground

                    NextLinkStyle()
                End If
            Else
                NextLinkStyle()
            End If
        End If
    End Sub

#End Region

#Region "NextLinkStyle"

    ''' <summary>
    ''' Sets the next available Link style
    ''' </summary>
    Private Sub NextLinkStyle()
        Dim linkStyle As eYearViewLinkStyle = CalendarView1.YearViewAppointmentLinkStyle

        linkStyle += 1

        If linkStyle > eYearViewLinkStyle.Style5 Then
            linkStyle = eYearViewLinkStyle.None
        End If

        CalendarView1.YearViewAppointmentLinkStyle = linkStyle
    End Sub

#End Region

#Region "YearViewDrawDayBackground"

    ''' <summary>
    ''' Draws the YearView day background
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub YearViewDrawDayBackground(ByVal sender As Object, ByVal e As YearViewDrawDayBackgroundEventArgs)
        If (e.YearMonth.DayIsSelected(e.Date.Day) = False) And (e.YearMonth.DayHasAppointments(e.Date.Day) = True) Then
            Dim color__1 As Color

            Select Case e.[Date].DayOfWeek
                Case DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday
                    color__1 = Color.Yellow
                    Exit Select

                Case DayOfWeek.Tuesday, DayOfWeek.Thursday
                    color__1 = Color.Coral
                    Exit Select
                Case Else

                    color__1 = Color.LightGreen
                    Exit Select
            End Select

            Using br As Brush = New SolidBrush(color__1)
                e.Graphics.FillRectangle(br, e.Bounds)
            End Using

            e.Cancel = True
        End If

    End Sub

#End Region

#End Region

#Region "InSideBarMouseUp"

    ''' <summary>
    ''' Handles SideBar MouseUp events
    ''' </summary>
    ''' <param name="view">BaseView</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub InSideBarMouseUp(ByVal view As BaseView, ByVal e As MouseEventArgs)
        InSideBarContextMenu.Tag = view

        ShowContextMenu(InSideBarContextMenu)
    End Sub

#End Region

#Region "InCondensedViewMouseUp"

    ''' <summary>
    ''' Handles Mouse Up events in the CondensedView
    ''' area of the control
    ''' </summary>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub InCondensedViewMouseUp(ByVal e As MouseEventArgs)
        ShowContextMenu(CondensedViewContextMenu)
    End Sub

#End Region

#End Region

#Region "AppointmentViewMouseUp"

    ''' <summary>
    ''' Handles AppointmentView MouseUp events
    ''' </summary>
    ''' <param name="sender">AppointmentView</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub AppointmentViewMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim view As AppointmentView = TryCast(sender, AppointmentView)

        ' Select the appointment
        view.IsSelected = True

        ' Let the user delete the appointment
        AppDeleteContextItem.Enabled = (view.Appointment.IsRecurringInstance = False)
        AppointmentContextMenu.Tag = view

        ShowContextMenu(AppointmentContextMenu)
    End Sub

#Region "miDelete_Click"

    ''' <summary>
    ''' Handles BaseView "Delete Appointment" 
    ''' ContextMenu selections
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AppDeleteContextItem.Click
        Dim mi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As AppointmentView = TryCast(mi.Parent.Tag, AppointmentView)

        If view IsNot Nothing Then
            CalendarView1.CalendarModel.Appointments.Remove(view.Appointment)
        End If
    End Sub

#End Region

#Region "Color item code"
    Private Sub CategoryColor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AppCatYellow.Click, AppCatRed.Click, AppCatPurple.Click, AppCatOrange.Click, AppCatGreen.Click, AppCatDefault.Click, AppCatBlue.Click
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As AppointmentView = TryCast(bi.Parent.Tag, AppointmentView)

        If view IsNot Nothing Then
            view.Appointment.CategoryColor = bi.Text
        End If
    End Sub

#End Region

#Region "Marker item code"
    Private Sub TimeMarker_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AppMarkerTentative.Click, AppMarkerOutOfOffice.Click, AppMarkerFree.Click, AppMarkerDefault.Click, AppMarkerBusy.Click
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim view As AppointmentView = TryCast(bi.Parent.Tag, AppointmentView)

        If view IsNot Nothing Then
            If bi.Text.Equals("Default") Then
                view.Appointment.TimeMarkedAs = Nothing
            Else
                view.Appointment.TimeMarkedAs = bi.Text
            End If
        End If
    End Sub

#End Region

#End Region

#Region "AllDayPanelMouseUp"

    ''' <summary>
    ''' Handles AllDayPanel MouseUp events
    ''' </summary>
    ''' <param name="sender">AllDayPanel</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub AllDayPanelMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        ' Let the user Shrink and expand the panel
        ' by 20 each time

        AllDayShrinkContextItem.Enabled = CalendarView1.FixedAllDayPanelHeight = -1 OrElse CalendarView1.FixedAllDayPanelHeight > 20

        AllDayGrowContextItem.Enabled = CalendarView1.FixedAllDayPanelHeight < 200
        AllDayReset.Enabled = CalendarView1.FixedAllDayPanelHeight <> -1
        AllDayPanelContextMenu.Tag = sender

        ShowContextMenu(AllDayPanelContextMenu)
    End Sub

    ''' <summary>
    ''' Handles "Shrink" ContextMenu selections
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miShrink_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AllDayShrinkContextItem.Click
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim panel As AllDayPanel = TryCast(bi.Parent.Tag, AllDayPanel)

        If CalendarView1.FixedAllDayPanelHeight = -1 Then
            CalendarView1.FixedAllDayPanelHeight = panel.PanelHeight - 20
        Else

            CalendarView1.FixedAllDayPanelHeight = Math.Max(20, CalendarView1.FixedAllDayPanelHeight - 20)
        End If
    End Sub

    ''' <summary>
    ''' Handles "Grow" ContextMenu selections
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miGrow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AllDayGrowContextItem.Click
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)
        Dim panel As AllDayPanel = TryCast(bi.Parent.Tag, AllDayPanel)

        If CalendarView1.FixedAllDayPanelHeight = -1 Then
            CalendarView1.FixedAllDayPanelHeight = panel.PanelHeight + 20
        Else

            CalendarView1.FixedAllDayPanelHeight = Math.Min(500, CalendarView1.FixedAllDayPanelHeight + 20)
        End If
    End Sub

    ''' <summary>
    ''' Handles "Reset" ContextMenu selections
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AllDayReset.Click
        CalendarView1.FixedAllDayPanelHeight = -1
    End Sub

#End Region

#Region "TimeRulerPanelMouseUp"

    ''' <summary>
    ''' Handles TimeRulerPanel MouseUp events
    ''' </summary>
    ''' <param name="sender">TimeRulerPanel</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub TimeRulerPanelMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        ShowContextMenu(TimeRulerPanelContextMenu)
    End Sub

    ''' <summary>
    ''' Handles "TimeSlotDuration" ContextMenu selections
    ''' </summary>
    ''' <param name="sender">MenuItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub miTimeDuration_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TimeDuration30.Click, buttonItem9.Click, buttonItem8.Click, buttonItem7.Click, buttonItem6.Click, buttonItem22.Click, buttonItem16.Click, buttonItem15.Click, buttonItem13.Click, buttonItem12.Click, buttonItem11.Click, TimeRulerPanelContextMenu.Click
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)

        Dim duration As Integer

        If Integer.TryParse(bi.Text, duration) = True Then
            CalendarView1.TimeSlotDuration = duration
        End If
    End Sub

#End Region

#Region "TimeLineHeaderPanelMouseUp"

    ''' <summary>
    ''' Handles Mouse Up events in the TimeLineHeaderPanel.
    ''' 
    ''' (The TimeLineHeaderPanel is the area at the top of the
    ''' TimeLineView that holds the Period and Interval Headers.)
    ''' </summary>
    ''' <param name="sender">TimeLineHeaderPanel</param>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub TimeLineHeaderPanelMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim tp As TimeLineHeaderPanel = TryCast(sender, TimeLineHeaderPanel)

        If tp IsNot Nothing Then
            Dim area As eViewArea = tp.GetViewAreaFromPoint(e.Location)

            If area = eViewArea.InPeriodHeader Then
                InPeriodHeaderMouseUp(e)

            ElseIf area = eViewArea.InIntervalHeader Then
                InIntervalHeaderMouseUp(e)
            End If
        End If
    End Sub

#Region "InPeriodHeaderMouseUp"

    ''' <summary>
    ''' Handles MouseUp events in the Period Header
    ''' </summary>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub InPeriodHeaderMouseUp(ByVal e As MouseEventArgs)
        ShowContextMenu(PeriodHeaderContextMenu)
    End Sub

#End Region

#Region "InPeriodHeaderHide_Click"

    ''' <summary>
    ''' Handles Period Header "Hide" menu selections
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub InPeriodHeaderHide_Click(ByVal sender As Object, ByVal e As EventArgs) Handles InPeriodHeaderHide.Click
        CalendarView1.TimeLineShowPeriodHeader = False
    End Sub

#End Region

#Region "PeriodHeaderContextMenu Period change"

    ''' <summary>
    ''' Handles PeriodHeaderContextMenu "Minute" selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPeriodMinutes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles btnPeriodMinutes.Click
        If btnPeriodMinutes.Checked = True Then
            CalendarView1.TimeLinePeriod = eTimeLinePeriod.Minutes
        End If
    End Sub

    ''' <summary>
    ''' Handles PeriodHeaderContextMenu "Hours" selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPeriodHours_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles btnPeriodHours.Click
        If btnPeriodHours.Checked = True Then
            CalendarView1.TimeLinePeriod = eTimeLinePeriod.Hours
        End If
    End Sub

    ''' <summary>
    ''' Handles PeriodHeaderContextMenu "Days" selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPeriodDays_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles btnPeriodDays.Click
        If btnPeriodDays.Checked = True Then
            CalendarView1.TimeLinePeriod = eTimeLinePeriod.Days
        End If
    End Sub

    ''' <summary>
    ''' Handles PeriodHeaderContextMenu "Years" selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPeriodYears_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles btnPeriodYears.Click
        If btnPeriodYears.Checked = True Then
            CalendarView1.TimeLinePeriod = eTimeLinePeriod.Years
        End If
    End Sub

#End Region

#Region "InIntervalHeaderMouseUp"

    ''' <summary>
    ''' Handles MouseUp events in the IntervalHeader
    ''' </summary>
    ''' <param name="e">MouseEventArgs</param>
    Private Sub InIntervalHeaderMouseUp(ByVal e As MouseEventArgs)
        ' Get rid of any previously added non-essential items

        For i As Integer = IntervalHeaderContextMenu.SubItems.Count - 1 To 3 Step -1
            IntervalHeaderContextMenu.SubItems.RemoveAt(i)
        Next

        ' If the Period Header is not visible, present the user
        ' with the items to be able to re-show it

        lblPeriodHeader2.Visible = (CalendarView1.TimeLineShowPeriodHeader = False)

        btnIntervalHeaderShow.Visible = (CalendarView1.TimeLineShowPeriodHeader = False)

        ' Add some Interval time selection options
        ' for the current selected Interval Period

        Select Case CalendarView1.TimeLinePeriod
            Case eTimeLinePeriod.Minutes
                AddIntervalMinutes()
                Exit Select

            Case eTimeLinePeriod.Hours
                AddIntervalHours()
                Exit Select

            Case eTimeLinePeriod.Days
                AddIntervalDays()
                Exit Select

            Case eTimeLinePeriod.Years
                AddIntervalYears()
                Exit Select
        End Select

        ShowContextMenu(IntervalHeaderContextMenu)
    End Sub

#Region "Interval time support"

#Region "AddIntervalMinutes"

    ''' <summary>
    ''' Adds Interval "Minute" items
    ''' </summary>
    Private Sub AddIntervalMinutes()
        For i As Integer = 1 To 60
            If 60 Mod i = 0 Then
                AddIntervalItem(i)
            End If
        Next
    End Sub

#End Region

#Region "AddIntervalHours"

    ''' <summary>
    ''' Adds Interval "Hour" items
    ''' </summary>
    Private Sub AddIntervalHours()
        For i As Integer = 1 To 24
            If 24 Mod i = 0 Then
                AddIntervalItem(i)
            End If
        Next
    End Sub

#End Region

#Region "AddIntervalDays"

    ''' <summary>
    ''' Adds Interval "Day" items
    ''' </summary>
    Private Sub AddIntervalDays()
        For i As Integer = 1 To 10
            AddIntervalItem(i)
        Next

        For i As Integer = 30 To 199 Step 30
            AddIntervalItem(i)
        Next

        AddIntervalItem(365)
    End Sub

#End Region

#Region "AddIntervalYears"

    ''' <summary>
    ''' Adds Interval "Year" items
    ''' </summary>
    Private Sub AddIntervalYears()
        For i As Integer = 1 To 10
            AddIntervalItem(i)
        Next
    End Sub

#End Region

#Region "AddIntervalItem"

    ''' <summary>
    ''' Adds individual Interval items
    ''' </summary>
    ''' <param name="i">Item to add</param>
    Private Sub AddIntervalItem(ByVal i As Integer)
        Dim bi As New ButtonItem("", i.ToString())

        AddHandler bi.Click, AddressOf bi_IntervalClick

        If CalendarView1.TimeLineInterval = i Then
            bi.Checked = True
        End If

        IntervalHeaderContextMenu.SubItems.Add(bi)
    End Sub

#End Region

#Region "bi_IntervalClick"

    ''' <summary>
    ''' Handles Interval time selections
    ''' </summary>
    ''' <param name="sender">ButtonItem</param>
    ''' <param name="e">EventArgs</param>
    Private Sub bi_IntervalClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim bi As ButtonItem = TryCast(sender, ButtonItem)

        Dim n As Integer

        If Integer.TryParse(bi.Text, n) = True Then
            CalendarView1.TimeLineInterval = n
        End If
    End Sub

#End Region

#End Region

#Region "btnIntervalHeaderShow_Click"

    ''' <summary>
    ''' Handles IntervalHeaderContextMenu "Show Header" selections
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnIntervalHeaderShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIntervalHeaderShow.Click
        CalendarView1.TimeLineShowPeriodHeader = True
    End Sub

#End Region

#End Region

#End Region

#Region "ShowContextMenu"

    ''' <summary>
    ''' Shows the given ContextMenu
    ''' </summary>
    ''' <param name="cm">ContextMenu to show</param>
    Private Sub ShowContextMenu(ByVal cm As ButtonItem)
        Dim pt As Point = Control.MousePosition
        cm.Popup(pt)
    End Sub

#End Region

#End Region

#Region "CondensedViewContextMenu support"

    ''' <summary>
    ''' Handles "AllResources" selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCondensedViewAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCondensedViewAll.Click
        ' AllResources - This selection enables Condensed
        ' views to be displayed in every visible TimeLine
        ' when Multiuser TimeLine views are displayed

        CalendarView1.TimeLineCondensedViewVisibility = eCondensedViewVisibility.AllResources
    End Sub

    ''' <summary>
    ''' Handles "SelectedResource" selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCondensedViewSelected_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCondensedViewSelected.Click
        ' SelectedResource - This selection enables Condensed
        ' views to be displayed only in the currently selected
        ' TimeLine view when Multiuser TimeLine views are displayed

        CalendarView1.TimeLineCondensedViewVisibility = eCondensedViewVisibility.SelectedResource
    End Sub

    ''' <summary>
    ''' Handles "Hidden" selection
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCondensedViewHidden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCondensedViewHidden.Click
        ' Hidden - This selection disables all Condensed
        ' views from being displayed

        CalendarView1.TimeLineCondensedViewVisibility = eCondensedViewVisibility.Hidden
    End Sub

#End Region

#Region "Color Scheme Selection"

    ''' <summary>
    ''' Selects the Office2007Blue color scheme
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Office2007Blue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Office2007Blue.Click
        StyleManager1.ManagerStyle = eStyle.Office2007Blue
    End Sub

    ''' <summary>
    ''' Selects the Office2007Silver color scheme
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Office2007Silver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Office2007Silver.Click
        StyleManager1.ManagerStyle = eStyle.Office2007Silver
    End Sub

    ''' <summary>
    ''' Selects the Office2007Black color scheme
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Office2007Black_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Office2007Black.Click
        StyleManager1.ManagerStyle = eStyle.Office2007Black
    End Sub

    ''' <summary>
    ''' Selects the Office2007VistaGlass color scheme
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Office2007VistaGlass_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Office2007VistaGlass.Click
        StyleManager1.ManagerStyle = eStyle.Office2007VistaGlass
    End Sub

    ''' <summary>
    ''' Selects the Office2010Blue color scheme
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Office2010Blue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Office2010Blue.Click
        StyleManager1.ManagerStyle = eStyle.Office2010Blue
    End Sub

    ''' <summary>
    ''' Selects the Office2010Silver color scheme
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Office2010Silver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Office2010Silver.Click
        StyleManager1.ManagerStyle = eStyle.Office2010Silver
    End Sub

    ''' <summary>
    ''' Selects the Windows7Blue color scheme
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Windows7Blue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Windows7Blue.Click
        'StyleManager1.ManagerStyle = eStyle.Windows7Blue
        Me.miDelete_Click(sender, e)
        cargarMievento()
    End Sub

#End Region

#Region "btnToday_Click"

    ''' <summary>
    ''' Sets the calendar view display to today's date
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnToday_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnToday.Click
        CalendarView1.ShowDate(DateTime.Today)
    End Sub

#End Region

#Region "btnNewAppointment_Click"

    ''' <summary>
    ''' Adds a new appointment to the calendar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNewAppointment_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewAppointment.Click
        ' Create new appointment and add it to the model
        ' Appointment will show up in the view automatically

        Dim startDate As DateTime = DateTime.Today.AddDays(3).AddHours(5)
        Dim endDate As DateTime = startDate.AddHours(2.5)

        If CalendarView1.DateSelectionStart.HasValue AndAlso CalendarView1.DateSelectionEnd.HasValue Then
            startDate = CalendarView1.DateSelectionStart.Value
            endDate = CalendarView1.DateSelectionEnd.Value
        End If

        Dim ap As Appointment = AddNewAppointment(startDate, endDate)

        ' Make sure the appointment is visible

        CalendarView1.EnsureVisible(ap)
    End Sub

#End Region

#Region "btnNewAllDay_Click"

    ''' <summary>
    ''' Adds a new All Day event to the calendar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNewAllDay_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewAllDay.Click
        ' All day or multi-day appointments simply have longer duration

        Dim appointment As New Appointment()

        appointment.StartTime = DateTime.Today
        appointment.EndTime = appointment.StartTime.AddDays(1)

        appointment.Subject = "All Day Event Appointment"
        appointment.CategoryColor = appointment.CategoryYellow

        ' Add appointment to the model and
        ' make sure it is visible

        CalendarView1.CalendarModel.Appointments.Add(appointment)
        CalendarView1.EnsureVisible(appointment)
    End Sub

#End Region

#Region "btnNewMultiDay_Click"

    ''' <summary>
    ''' Adds a new Multi-Day event to the calendar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNewMultiDay_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewMultiDay.Click
        ' Multi-day appointment

        Dim appointment As New Appointment()

        appointment.StartTime = DateTime.Today
        appointment.EndTime = appointment.StartTime.AddDays(2.5)
        appointment.Subject = "Multiple Days Appointment"

        ' Add appointment to the model and
        ' make sure it is visible

        CalendarView1.CalendarModel.Appointments.Add(appointment)
        CalendarView1.EnsureVisible(appointment)
    End Sub

#End Region

#Region "btnNewRecurring_Click"

    ''' <summary>
    ''' Adds a new recurring appointment to the calendar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNewRecurring_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewRecurring.Click
        ' Multi-day appointment

        Dim appointment As New Appointment()

        appointment.Subject = "Recurring Appointment every 3rd week day"
        appointment.StartTime = DateTime.Today.AddHours(9)
        appointment.EndTime = appointment.StartTime.AddHours(1)

        appointment.TimeMarkedAs = appointment.TimerMarkerOutOfOffice
        appointment.CategoryColor = appointment.CategoryOrange

        ' Set recurrence type to weekly
        appointment.Recurrence = New AppointmentRecurrence()
        appointment.Recurrence.RecurrenceType = eRecurrencePatternType.Daily

        ' Recurrence properties are changed then on respective object Daily, Weekly, Monthly, Yearly
        appointment.Recurrence.Daily.RepeatOnDaysOfWeek = eDailyRecurrenceRepeat.All

        ' Repeat every 3 days
        appointment.Recurrence.Daily.RepeatInterval = 3

        ' End recurrence 30 days from today
        appointment.Recurrence.RangeEndDate = DateTime.Today.AddDays(30)

        ' Add appointment to the model and
        ' make sure it is visible

        CalendarView1.CalendarModel.Appointments.Add(appointment)
        CalendarView1.EnsureVisible(appointment)
    End Sub

#End Region

#Region "btnNewReminder_Click"

    ''' <summary>
    ''' Adds a new appointment with a reminder in 2 minutes
    ''' </summary>
    ''' <param name="sender">btnNewReminder</param>
    ''' <param name="e">EventArgs</param>
    Private Sub btnNewReminder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewReminder.Click
        ' Create new appointment with reminder in 2 minutes

        Dim appointment As New Appointment()

        appointment.Subject = "Appointment with reminder"
        appointment.StartTime = DateTime.Now.AddHours(2)
        appointment.EndTime = appointment.StartTime.AddMinutes(30)
        appointment.OwnerKey = CalendarView1.SelectedOwner

        appointment.CategoryColor = appointment.CategoryRed
        appointment.TimeMarkedAs = appointment.TimerMarkerTentative

        ' Create new reminder and add it to Reminders collection
        appointment.Reminders.Add(New Reminder(DateTime.Now.AddMinutes(2)))

        ' Add appointment to the model
        CalendarView1.CalendarModel.Appointments.Add(appointment)

        ' Make appointment visible in current view
        CalendarView1.EnsureVisible(appointment)
    End Sub

#End Region

#Region "btnNewStartReached_Click"

    ''' <summary>
    ''' Adds a new appointment with Start Notification
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNewStartReached_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewStartReached.Click
        ' Create new appointment with start time notification

        Dim appointment As New Appointment()

        appointment.Subject = "Appointment with StartTimeReached Notification"
        appointment.StartTime = DateTime.Now.AddMinutes(2)
        appointment.EndTime = appointment.StartTime.AddHours(3)
        appointment.OwnerKey = CalendarView1.SelectedOwner

        appointment.CategoryColor = appointment.CategoryPurple
        appointment.TimeMarkedAs = appointment.TimerMarkerTentative

        ' Set our StartTimeAction to enable event notification
        appointment.StartTimeAction = eStartTimeAction.StartTimeReachedEvent

        ' Add appointment to the model
        CalendarView1.CalendarModel.Appointments.Add(appointment)

        ' Make appointment visible in current view
        CalendarView1.EnsureVisible(appointment)
    End Sub

#End Region

#Region "calendarView1_SelectedViewChanged"

    ''' <summary>
    ''' Processes CalendarView SelectedViewChanged events
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub calendarView1_SelectedViewChanged(ByVal sender As Object, ByVal e As SelectedViewEventArgs) Handles CalendarView1.SelectedViewChanged
        Select Case e.NewValue
            Case eCalendarView.Day
                ButtonDay.Checked = True
                Exit Select

            Case eCalendarView.Week
                ButtonWeek.Checked = True
                Exit Select

            Case eCalendarView.Month
                ButtonMonth.Checked = True
                Exit Select

            Case eCalendarView.Year
                btnYear.Checked = True
                Exit Select

            Case eCalendarView.TimeLine
                btnTimeLine.Checked = True
                Exit Select
        End Select
    End Sub

#End Region



    Private Sub c_eventos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        principal.mp_eventos.Enabled = True

    End Sub

    Private Sub ButtonItem1_Click(sender As Object, e As EventArgs) Handles ButtonItem1.Click
        eliminaSalones()
        cargarsalones_filtro(txtsalon.Text)
        cargareventos()
    End Sub

    Private Sub cmd_reservas_Click(sender As Object, e As EventArgs) Handles cmd_reservas.Click
        Dim isformcargado As Boolean = False
        Dim mForm As Form
        For Each mForm In principal.MdiChildren
            If mForm.Name = "rpt_reservas" Then
                If mForm.WindowState = FormWindowState.Minimized Then
                    mForm.WindowState = FormWindowState.Normal
                Else
                    mForm.BringToFront()
                End If
                isformcargado = True
                Exit For
            End If
        Next
        mForm = Nothing

        If isformcargado = False Then
            rpt_reservas.MdiParent = principal


        Else

            rpt_reservas.Activate()
        End If

        rpt_reservas.Show()
    End Sub

    Private Sub CalendarView1_ItemClick(sender As Object, e As EventArgs) Handles CalendarView1.ItemClick

    End Sub

    Private Sub c_eventos_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ButtonItem18_Click(sender As Object, e As EventArgs) Handles ButtonItem18.Click
        Dim isformcargado As Boolean = False
        Dim mForm As Form
        For Each mForm In principal.MdiChildren
            If mForm.Name = "rpt_consolidar" Then
                If mForm.WindowState = FormWindowState.Minimized Then
                    mForm.WindowState = FormWindowState.Normal
                Else
                    mForm.BringToFront()
                End If
                isformcargado = True
                Exit For
            End If
        Next
        mForm = Nothing

        If isformcargado = False Then
            rpt_consolidar.MdiParent = principal

        Else
            rpt_consolidar.Activate()
        End If
        rpt_consolidar.Show()
    End Sub
End Class
