import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { SensorValue } from './models/SensorValue';
import { SensorValueService } from './services/SensorValue.service';
import { environment } from 'environments/environment';
import { SensorService } from '../sensor/services/Sensor.service';
import { Sensor } from '../sensor/models/Sensor';

declare var jQuery: any;

@Component({
	selector: 'app-sensorValue',
	templateUrl: './sensorValue.component.html',
	styleUrls: ['./sensorValue.component.scss']
})
export class SensorValueComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','sensorId','value','dateTime', 'update','delete'];

	sensorValueList:SensorValue[];
	sensorValue:SensorValue=new SensorValue();

	sensorValueAddForm: FormGroup;


	sensorValueId:number;

	sensorList:Sensor[];

	constructor(private sensorValueService:SensorValueService, private sensorService:SensorService,private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getSensorValueList();
    }

	ngOnInit() {

		this.getSensorList();
		this.createSensorValueAddForm();
	}

	getSensorList(){
		this.sensorService.getSensorList().subscribe(data=>{
			this.sensorList=data
		})
	}


	getSensorValueList() {
		this.sensorValueService.getSensorValueList().subscribe(data => {
			this.sensorValueList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.sensorValueAddForm.valid) {
			this.sensorValue = Object.assign({}, this.sensorValueAddForm.value)

			if (this.sensorValue.id == 0)
				this.addSensorValue();
			else
				this.updateSensorValue();
		}

	}

	addSensorValue(){

		this.sensorValueService.addSensorValue(this.sensorValue).subscribe(data => {
			this.getSensorValueList();
			this.sensorValue = new SensorValue();
			jQuery('#sensorvalue').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.sensorValueAddForm);

		})

	}

	updateSensorValue(){

		this.sensorValueService.updateSensorValue(this.sensorValue).subscribe(data => {

			var index=this.sensorValueList.findIndex(x=>x.id==this.sensorValue.id);
			this.sensorValueList[index]=this.sensorValue;
			this.dataSource = new MatTableDataSource(this.sensorValueList);
            this.configDataTable();
			this.sensorValue = new SensorValue();
			jQuery('#sensorvalue').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.sensorValueAddForm);

		})

	}

	createSensorValueAddForm() {
		this.sensorValueAddForm = this.formBuilder.group({		
			id : [0],
sensorId : [0, Validators.required],
value : [0, Validators.required],
dateTime : [null, Validators.required]
		})
	}

	deleteSensorValue(sensorValueId:number){
		this.sensorValueService.deleteSensorValue(sensorValueId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.sensorValueList=this.sensorValueList.filter(x=> x.id!=sensorValueId);
			this.dataSource = new MatTableDataSource(this.sensorValueList);
			this.configDataTable();
		})
	}

	getSensorValueById(sensorValueId:number){
		this.clearFormGroup(this.sensorValueAddForm);
		this.sensorValueService.getSensorValueById(sensorValueId).subscribe(data=>{
			this.sensorValue=data;
			this.sensorValueAddForm.patchValue(data);
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
