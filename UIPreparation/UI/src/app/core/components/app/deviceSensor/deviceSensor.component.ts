import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { DeviceSensor } from './models/DeviceSensor';
import { DeviceSensorService } from './services/DeviceSensor.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-deviceSensor',
	templateUrl: './deviceSensor.component.html',
	styleUrls: ['./deviceSensor.component.scss']
})
export class DeviceSensorComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','deviceId','sensorId', 'update','delete'];

	deviceSensorList:DeviceSensor[];
	deviceSensor:DeviceSensor=new DeviceSensor();

	deviceSensorAddForm: FormGroup;


	deviceSensorId:number;

	constructor(private deviceSensorService:DeviceSensorService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getDeviceSensorList();
    }

	ngOnInit() {

		this.createDeviceSensorAddForm();
	}


	getDeviceSensorList() {
		this.deviceSensorService.getDeviceSensorList().subscribe(data => {
			this.deviceSensorList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.deviceSensorAddForm.valid) {
			this.deviceSensor = Object.assign({}, this.deviceSensorAddForm.value)

			if (this.deviceSensor.id == 0)
				this.addDeviceSensor();
			else
				this.updateDeviceSensor();
		}

	}

	addDeviceSensor(){

		this.deviceSensorService.addDeviceSensor(this.deviceSensor).subscribe(data => {
			this.getDeviceSensorList();
			this.deviceSensor = new DeviceSensor();
			jQuery('#devicesensor').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.deviceSensorAddForm);

		})

	}

	updateDeviceSensor(){

		this.deviceSensorService.updateDeviceSensor(this.deviceSensor).subscribe(data => {

			var index=this.deviceSensorList.findIndex(x=>x.id==this.deviceSensor.id);
			this.deviceSensorList[index]=this.deviceSensor;
			this.dataSource = new MatTableDataSource(this.deviceSensorList);
            this.configDataTable();
			this.deviceSensor = new DeviceSensor();
			jQuery('#devicesensor').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.deviceSensorAddForm);

		})

	}

	createDeviceSensorAddForm() {
		this.deviceSensorAddForm = this.formBuilder.group({		
			id : [0],
deviceId : [0, Validators.required],
sensorId : [0, Validators.required]
		})
	}

	deleteDeviceSensor(deviceSensorId:number){
		this.deviceSensorService.deleteDeviceSensor(deviceSensorId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.deviceSensorList=this.deviceSensorList.filter(x=> x.id!=deviceSensorId);
			this.dataSource = new MatTableDataSource(this.deviceSensorList);
			this.configDataTable();
		})
	}

	getDeviceSensorById(deviceSensorId:number){
		this.clearFormGroup(this.deviceSensorAddForm);
		this.deviceSensorService.getDeviceSensorById(deviceSensorId).subscribe(data=>{
			this.deviceSensor=data;
			this.deviceSensorAddForm.patchValue(data);
		})
	}


	clearFormGroup(group: FormGroup) {

		group.markAsUntouched();
		group.reset();

		Object.keys(group.controls).forEach(key => {
			group.get(key).setErrors(null);
			if (key == 'id')
				group.get(key).setValue(0);
		});
	}

	checkClaim(claim:string):boolean{
		return this.authService.claimGuard(claim)
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
