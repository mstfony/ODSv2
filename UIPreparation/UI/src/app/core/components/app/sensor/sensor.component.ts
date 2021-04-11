import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Sensor } from './models/Sensor';
import { SensorService } from './services/Sensor.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-sensor',
	templateUrl: './sensor.component.html',
	styleUrls: ['./sensor.component.scss']
})
export class SensorComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','name','alias', 'update','delete'];

	sensorList:Sensor[];
	sensor:Sensor=new Sensor();

	sensorAddForm: FormGroup;


	sensorId:number;

	constructor(private sensorService:SensorService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getSensorList();
    }

	ngOnInit() {

		this.createSensorAddForm();
	}


	getSensorList() {
		this.sensorService.getSensorList().subscribe(data => {
			this.sensorList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.sensorAddForm.valid) {
			this.sensor = Object.assign({}, this.sensorAddForm.value)

			if (this.sensor.id == 0)
				this.addSensor();
			else
				this.updateSensor();
		}

	}

	addSensor(){

		this.sensorService.addSensor(this.sensor).subscribe(data => {
			this.getSensorList();
			this.sensor = new Sensor();
			jQuery('#sensor').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.sensorAddForm);

		})

	}

	updateSensor(){

		this.sensorService.updateSensor(this.sensor).subscribe(data => {

			var index=this.sensorList.findIndex(x=>x.id==this.sensor.id);
			this.sensorList[index]=this.sensor;
			this.dataSource = new MatTableDataSource(this.sensorList);
            this.configDataTable();
			this.sensor = new Sensor();
			jQuery('#sensor').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.sensorAddForm);

		})

	}

	createSensorAddForm() {
		this.sensorAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required],
alias : ["", Validators.required]
		})
	}

	deleteSensor(sensorId:number){
		this.sensorService.deleteSensor(sensorId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.sensorList=this.sensorList.filter(x=> x.id!=sensorId);
			this.dataSource = new MatTableDataSource(this.sensorList);
			this.configDataTable();
		})
	}

	getSensorById(sensorId:number){
		this.clearFormGroup(this.sensorAddForm);
		this.sensorService.getSensorById(sensorId).subscribe(data=>{
			this.sensor=data;
			this.sensorAddForm.patchValue(data);
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
