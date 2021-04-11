import { Component, AfterViewInit, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { AlertifyService } from "app/core/services/alertify.service";
import { LookUpService } from "app/core/services/lookUp.service";
import { AuthService } from "app/core/components/admin/login/services/auth.service";
import { AlertAction } from "./models/AlertAction";
import { AlertActionService } from "./services/AlertAction.service";
import { environment } from "environments/environment";
import { SensorSetting } from "../sensorSetting/models/SensorSetting";
import { Alert } from "../alert/models/Alert";
import { SensorSettingService } from "../sensorSetting/services/SensorSetting.service";
import { AlertService } from "../alert/services/Alert.service";

declare var jQuery: any;

@Component({
  selector: "app-alertAction",
  templateUrl: "./alertAction.component.html",
  styleUrls: ["./alertAction.component.scss"],
})
export class AlertActionComponent implements AfterViewInit, OnInit {
  dataSource: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = [
    "id",
    "sensorSettingId",
    "alertId",
    "message",
    "update",
    "delete",
  ];

  alertActionList: AlertAction[];
  alertAction: AlertAction = new AlertAction();

  alertActionAddForm: FormGroup;

  alertActionId: number;

  sensorSettingList: SensorSetting[];
  alertList: Alert[];

  constructor(
    private alertActionService: AlertActionService,
    private sensorSettingService: SensorSettingService,
    private alertService: AlertService,
    private lookupService: LookUpService,
    private alertifyService: AlertifyService,
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {}

  ngAfterViewInit(): void {
    this.getAlertActionList();
  }

  ngOnInit() {
    this.getSensorSettingList();
    this.getAlertList();
    this.createAlertActionAddForm();
  }

  getSensorSettingList() {
    this.sensorSettingService.getSensorSettingList().subscribe((data) => {
      this.sensorSettingList = data;
      console.log(data);
    });
  }

  getAlertList() {
    this.alertService.getAlertList().subscribe((data) => {
      this.alertList = data;
      console.log(data);
    });
  }

  getAlertActionList() {
    this.alertActionService.getAlertActionList().subscribe((data) => {
      this.alertActionList = data;
	  console.log(data);
      this.dataSource = new MatTableDataSource(data);
      this.configDataTable();
    });
  }

  save() {
    if (this.alertActionAddForm.valid) {
      this.alertAction = Object.assign({}, this.alertActionAddForm.value);

      if (this.alertAction.id == 0) this.addAlertAction();
      else this.updateAlertAction();
    }
  }

  addAlertAction() {
    this.alertActionService
      .addAlertAction(this.alertAction)
      .subscribe((data) => {
        this.getAlertActionList();
        this.alertAction = new AlertAction();
        jQuery("#alertaction").modal("hide");
        this.alertifyService.success(data);
        this.clearFormGroup(this.alertActionAddForm);
      });
  }

  updateAlertAction() {
    this.alertActionService
      .updateAlertAction(this.alertAction)
      .subscribe((data) => {
        var index = this.alertActionList.findIndex(
          (x) => x.id == this.alertAction.id
        );
        this.alertActionList[index] = this.alertAction;
        this.dataSource = new MatTableDataSource(this.alertActionList);
        this.configDataTable();
        this.alertAction = new AlertAction();
        jQuery("#alertaction").modal("hide");
        this.alertifyService.success(data);
        this.clearFormGroup(this.alertActionAddForm);
      });
  }

  createAlertActionAddForm() {
    this.alertActionAddForm = this.formBuilder.group({
      id: [0],
      sensorSettingId: [0, Validators.required],
      alertId: [0, Validators.required],
      message: ["", Validators.required],
    });
  }

  deleteAlertAction(alertActionId: number) {
    this.alertActionService
      .deleteAlertAction(alertActionId)
      .subscribe((data) => {
        this.alertifyService.success(data.toString());
        this.alertActionList = this.alertActionList.filter(
          (x) => x.id != alertActionId
        );
        this.dataSource = new MatTableDataSource(this.alertActionList);
        this.configDataTable();
      });
  }

  getAlertActionById(alertActionId: number) {
    this.clearFormGroup(this.alertActionAddForm);
    this.alertActionService
      .getAlertActionById(alertActionId)
      .subscribe((data) => {
        this.alertAction = data;
        this.alertActionAddForm.patchValue(data);
      });
  }

  clearFormGroup(group: FormGroup) {
    group.markAsUntouched();
    group.reset();

    Object.keys(group.controls).forEach((key) => {
      group.get(key).setErrors(null);
      if (key == "id") group.get(key).setValue(0);
    });
  }

  checkClaim(claim: string): boolean {
    return this.authService.claimGuard(claim);
  }

  configDataTable(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
