Imports Vivei.Tools.Core.UI
Imports System.Collections.ObjectModel

Public Class StandardAnalysisView

    Private _SheetControl As unvell.ReoGrid.ReoGridControl

    Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

        Dim q = From src In ReservingPlugin.Instance.CurrentProject.DataSources
               Select New StandardAnalysisSource(src)

        Me.DataContext = q.ToArray()

        _SheetControl = New unvell.ReoGrid.ReoGridControl()
        '_SheetControl.MinWidth = 100
        '_SheetControl.MinHeight = 100

        Grid.SetRow(_SheetControl, 1)

        RootGrid.Children.Add(_SheetControl)


    End Sub


    Private _CoreData() As TriangleCell
    Private _Bornes() As Integer
    Private _NonCumule(,) As Double
    Private _Cumule(,) As Double
    Private _Coefficient(,) As Double
    Private _Facteur() As Double

    Private Function filterRowByField(ByVal row As System.Data.DataRow, ByVal field As StandardAnalysisFilterField) As Boolean
        For Each item In field.SelectedItems
            If Object.Equals(row(field.DataModelProperty.SourceColumn), item.Value) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function filterRowByManyField(ByVal row As System.Data.DataRow, ByVal fields As IEnumerable(Of StandardAnalysisFilterField)) As Boolean
        For Each item In fields
            If Not filterRowByField(row, item) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Function GetTriangleCells(ByVal src As StandardAnalysisSource, ByVal Survenance As System.Data.DataColumn, ByVal Sinistre As System.Data.DataColumn, ByVal Deroulement As System.Data.DataColumn, ByVal filters As StandardAnalysisFilterField()) As IEnumerable(Of TriangleCell)
        Return From r In src.DataSource.Data
                                  Where filterRowByManyField(r, filters) _
                                  And Not r.IsNull(Survenance) _
                                  And Not r.IsNull(Deroulement)
                                  Order By CInt(r(Survenance)), CInt(r(Deroulement))
                                  Group By X = CInt(r(Survenance)), Y = CInt(r(Deroulement))
                                  Into V = Sum(If(r.IsNull(Sinistre), 0.0, CDbl(r(Sinistre))))
                                  Select New TriangleCell(X, Y, V)
    End Function
    Private Sub UpdateButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        'For Each elt In CType(Me.DataContext, IEnumerable(Of StandardAnalysisSource))
        '    For Each fld In elt
        '        fld.SelectedItems.Clear()
        '    Next
        'Next

        'Return

        Dim src As StandardAnalysisSource = DataSourceComboBox.SelectedItem

        Dim Survenance = src.DataSource.GetColumnByUsage("Survenance")
        Dim Sinistre = src.DataSource.GetColumnByUsage("Sinistre")
        Dim Deroulement = src.DataSource.GetColumnByUsage("Deroulement")

        Dim filters = (From fld In src
                       Where (Not fld(0).Selected) And (fld.SelectedItems.Count > 0)
                       Select fld).ToArray()

        Dim fileredRows = GetTriangleCells(src, Survenance, Sinistre, Deroulement, filters)

        Me._CoreData = fileredRows.ToArray()
        Dim sheet = Me._SheetControl.Worksheets(0)

        If Me._CoreData.Length = 0 Then
            sheet.ClearRangeContent(New unvell.ReoGrid.ReoGridRange(0, 0, 200, 200), unvell.ReoGrid.CellElementFlag.All)
            'sheet.out
            Return
        End If

        Me._Bornes = New Integer() _
        { _
            _CoreData.Min(Function(c) c.X), _
             _CoreData.Max(Function(c) c.X), _
            _CoreData.Min(Function(c) c.Y), _
             _CoreData.Max(Function(c) c.Y)
        }


        sheet.ClearRangeContent(New unvell.ReoGrid.ReoGridRange(0, 0, 200, 200), unvell.ReoGrid.CellElementFlag.All)
        Dim maxSurv = Math.Min(20, Me._Bornes(1))
        Dim triangleSize = maxSurv - Me._Bornes(0) + 1

        ReDim _NonCumule(triangleSize, triangleSize)
        For Each c In _CoreData
            If c.X <= maxSurv And c.Y <= maxSurv And c.X >= Me._Bornes(0) And c.Y >= Me._Bornes(0) Then
                _NonCumule(c.X - Me._Bornes(0), c.Y - Me._Bornes(0)) = c.V
            End If
        Next

        ReDim _Cumule(triangleSize, triangleSize)
        For i = 0 To triangleSize - 1
            For j = 0 To triangleSize - 1
                If j = 0 Then
                    _Cumule(i, j) = _NonCumule(i, j)
                Else
                    'If j <= i Then
                    _Cumule(i, j) = _Cumule(i, j - 1) + _NonCumule(i, j)
                    '    Else
                    '    _Cumule(i, j) = Double.NaN
                    'End If
                End If
            Next
        Next

        ReDim Me._Coefficient(triangleSize, triangleSize)
        For i = 0 To triangleSize - 1
            For j = 0 To triangleSize - 1
                If j = 0 Then
                    _Coefficient(i, j) = Double.NaN
                Else
                    If _Cumule(i, j - 1) = 0 Then
                        _Coefficient(i, j) = Double.NaN
                    Else
                        _Coefficient(i, j) = _Cumule(i, j) / _Cumule(i, j - 1)
                    End If
                End If
            Next
        Next

        ReDim Me._Facteur(triangleSize * 2)

        For j = 0 To triangleSize - 1

            Dim cc = 0.0
            Dim cs = 0.0
            Dim cp = 0.0
            Dim cscp = 0.0

            For i = 0 To triangleSize - 1


                If j = 0 Then
                    _Facteur(j) = Double.NaN
                Else
                    If Not Double.IsNaN(_Coefficient(i, j)) And (j <= i) Then
                        cc += 1
                        cp += _Cumule(i, j)
                        cs += _Coefficient(i, j)
                        cscp += _Coefficient(i, j) * _Cumule(i, j)
                    End If
                End If
            Next

            If cc > 0 And cs > 0 And cp > 0 And cscp > 0 Then

                _Facteur(j) = cs / cc
                _Facteur(triangleSize + j) = cscp / cp
            Else
                _Facteur(j) = Double.NaN
            End If
        Next

        For i = 0 To triangleSize - 1
            For j = 0 To triangleSize - 1
                'If j = 0 Then
                '    _Cumule(i, j) = _NonCumule(i, j)
                'Else
                '    If j <= i Then
                '        _Cumule(i, j) = _Cumule(i, j - 1) + _NonCumule(i, j)
                '    End If
                'End If
                If j <= i Then
                    sheet(2 + triangleSize - 1 - i, 2 + j) = Math.Floor(_NonCumule(i, j))
                    sheet(2 + triangleSize - 1 - i + triangleSize + 2, 2 + j) = Math.Floor(_Cumule(i, j))
                    sheet(2 + triangleSize - 1 - i + 2 * (triangleSize + 2), 2 + j) = If(Double.IsNaN(_Coefficient(i, j)), "", Math.Round(_Coefficient(i, j), 4))

                End If


            Next

            sheet(2 + triangleSize - 1 - i, 1) = i + 1
            sheet(2 + triangleSize - 1 - i + triangleSize + 2, 1) = i + 1
            sheet(2 + triangleSize - 1 - i + 2 * (triangleSize + 2), 1) = i + 1

            sheet(2 + triangleSize - 1 - triangleSize, 2 + i) = i + 1
            sheet(2 + triangleSize - 1 - triangleSize + triangleSize + 2, 2 + i) = i + 1
            sheet(2 + triangleSize - 1 - triangleSize + 2 * (triangleSize + 2), 2 + i) = i + 1

            sheet(2 + triangleSize + 2 + triangleSize - 1 - triangleSize + 2 * (triangleSize + 2), 2 + i) = If(Double.IsNaN(_Facteur(i)), "", Math.Round(_Facteur(i), 4))
            sheet(2 + 2 + triangleSize + 2 + triangleSize - 1 - triangleSize + 2 * (triangleSize + 2), 2 + i) = If(Double.IsNaN(_Facteur(i + triangleSize)), "", Math.Round(_Facteur(i + triangleSize), 4))
        Next

        'sheet.GetOutlines(unvell.ReoGrid.RowOrColumn.Row).Clear()

        If sheet.GetOutlines(unvell.ReoGrid.RowOrColumn.Row) IsNot Nothing Then
            sheet.GetOutlines(unvell.ReoGrid.RowOrColumn.Row).Clear()

        End If

        sheet.AddOutline(unvell.ReoGrid.RowOrColumn.Row, 2 + 0, triangleSize)
        sheet.AddOutline(unvell.ReoGrid.RowOrColumn.Row, 2 + triangleSize + 2, triangleSize)
        sheet.AddOutline(unvell.ReoGrid.RowOrColumn.Row, 2 + 2 * (triangleSize + 2), triangleSize)

        Try
            sheet.SetRangeBorders(2, 2, triangleSize, triangleSize, unvell.ReoGrid.BorderPositions.LeftRight, unvell.ReoGrid.BorderStyle.BlackSolid)
            sheet.SetRangeBorders(2 + triangleSize + 2, 2, triangleSize, triangleSize, unvell.ReoGrid.BorderPositions.LeftRight, unvell.ReoGrid.BorderStyle.BlackSolid)
            sheet.SetRangeBorders(2 + 2 * (triangleSize + 2), 2, triangleSize, triangleSize, unvell.ReoGrid.BorderPositions.LeftRight, unvell.ReoGrid.BorderStyle.BlackSolid)
            sheet.SetRangeBorders(2, 2, triangleSize, triangleSize, unvell.ReoGrid.BorderPositions.TopBottom, unvell.ReoGrid.BorderStyle.BlackSolid)
            sheet.SetRangeBorders(2 + triangleSize + 2, 2, triangleSize, triangleSize, unvell.ReoGrid.BorderPositions.TopBottom, unvell.ReoGrid.BorderStyle.BlackSolid)
            sheet.SetRangeBorders(2 + 2 * (triangleSize + 2), 2, triangleSize, triangleSize, unvell.ReoGrid.BorderPositions.TopBottom, unvell.ReoGrid.BorderStyle.BlackSolid)

            sheet.AddOutline(unvell.ReoGrid.RowOrColumn.Row, 2 + 0, triangleSize)
        Catch ex As Exception

        End Try
    End Sub

    Public Structure TriangleCell
        Public X As Integer
        Public Y As Integer
        Public V As Double


        Sub New(ByVal X As Integer, ByVal Y As Integer, ByVal V As Double)
            ' TODO: Complete member initialization 
            Me.X = X
            Me.Y = Y
            Me.V = V
        End Sub

    End Structure
End Class

Public Class StandardAnalysisSource
    Inherits ObservableCollection(Of StandardAnalysisFilterField)

    Private _src As Model.DataSource

    Sub New(ByVal src As Model.DataSource)
        ' TODO: Complete member initialization 
        _src = src

        Dim q = From c In src.Model
                Where (c.Usage = "Segmentation" Or c.Usage = "Garantie") And c.SourceColumn IsNot Nothing
                Select c

        For Each elt In q
            Me.Add(New StandardAnalysisFilterField(Me, elt))

        Next

    End Sub

    Public ReadOnly Property DataSource As Model.DataSource
        Get
            Return _src
        End Get
    End Property
End Class

Public Class StandardAnalysisFilter
    Inherits ObservableCollection(Of StandardAnalysisFilterField)

End Class
Public Class StandardAnalysisFilterField
    Inherits ObservableCollection(Of StandardAnalysisFilterFieldItem)


    Private _Caption As String
    Private _standardAnalysisSource As StandardAnalysisSource
    Private _elt As DataModelProperty

    Sub New(ByVal standardAnalysisSource As StandardAnalysisSource, ByVal elt As DataModelProperty)
        ' TODO: Complete member initialization 
        _standardAnalysisSource = standardAnalysisSource
        _elt = elt
        _Caption = _elt.Name

        Me.Add(New StandardAnalysisFilterFieldItem(Me))

        Dim q = From r In standardAnalysisSource.DataSource.Data
                Where Not r.IsNull(elt.SourceColumn.ColumnName)
                Order By r(elt.SourceColumn.ColumnName)
                Select r(elt.SourceColumn.ColumnName) Distinct

        For Each v In q
            Me.Add(New StandardAnalysisFilterFieldItem(Me, v))
            SelectedItems.Add(Me(0))
        Next


    End Sub

    Public ReadOnly Property DataModelProperty As DataModelProperty
        Get
            Return _elt
        End Get
    End Property
    Public Property Caption As String
        Get
            Return _Caption
        End Get
        Set(ByVal value As String)
            _Caption = value
            OnPropertyChanged(New ComponentModel.PropertyChangedEventArgs("Caption"))
        End Set
    End Property

    Friend Property SelectedItems As New List(Of StandardAnalysisFilterFieldItem)


End Class
Public Class StandardAnalysisFilterFieldItem
    Inherits UIObject


    Private _Value As Object
    Private _Caption As String
    Private _standardAnalysisFilterField As StandardAnalysisFilterField

    Sub New(ByVal standardAnalysisFilterField As StandardAnalysisFilterField)
        ' TODO: Complete member initialization 
        _standardAnalysisFilterField = standardAnalysisFilterField
        _Caption = "(Tous)"
        _Value = Nothing
        _Selected = True
    End Sub

    Sub New(ByVal standardAnalysisFilterField As StandardAnalysisFilterField, ByVal v As Object)
        ' TODO: Complete member initialization 
        _standardAnalysisFilterField = standardAnalysisFilterField
        _Caption = v.ToString()
        _Value = v
        _Selected = False
    End Sub

    Public Property Caption As String
        Get
            Return _Caption
        End Get
        Set(ByVal value As String)
            _Caption = value
            OnPropertyChanged("Caption")
        End Set
    End Property

    Private _Selected As Boolean
    Public Property Selected As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal value As Boolean)
            If _Selected = value Then Return
            _Selected = value
            If (_Selected) Then
                Me._standardAnalysisFilterField.SelectedItems.Add(Me)
            Else
                Me._standardAnalysisFilterField.SelectedItems.Remove(Me)
            End If
            OnPropertyChanged("Selected")
        End Set
    End Property

    Public Property Value As Object
        Get
            Return _Value
        End Get
        Set(ByVal value As Object)
            _Value = value
            OnPropertyChanged("Value")
        End Set
    End Property

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Dim other = TryCast(obj, StandardAnalysisFilterFieldItem)

        If other IsNot Nothing Then Return Object.Equals(Me._Value, other._Value)

        Return MyBase.Equals(obj)
    End Function

    Public Overrides Function GetHashCode() As Integer
        If _Value Is Nothing Then Return 0
        Return _Value.GetHashCode()
    End Function

    Public Overrides Function ToString() As String
        If _Value Is Nothing Then Return "(Tous)"
        Return _Value.ToString()
    End Function
End Class