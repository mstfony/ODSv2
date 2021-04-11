import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { SensorSetting } from './models/SensorSetting';
import { SensorSettingService } from './services/SensorSetting.service';
import { environment } from 'environments/environment';
import { Setting } from '../setting/models/Setting';
import { Sensor } from '../sensor/models/Sensor';
import { SettingService } from '../setting/services/Setting.service';
import { SensorService } from '../sensor/services/Sensor.service';

declare var jQuery: any;

@Component({
	selector: 'app-sensorSetting',
	templateUrl: './sensorSetting.component.html',
	styleUrls: ['./sensorSetting.component.scss']
})
export class SensorSettingComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','sensorId','settingId', 'update','delete'];

	sensorSettingList:SensorSetting[];
	sensorSetting:SensorSetting=new SensorSetting();

	sensorSettingAddForm: FormGroup;


	sensorSettingId:number;

	settingList:Setting[];
	sensorList:Sensor[];

	constructor(private sensorSettingService:SensorSettingService, private lookupService:LookUpService,private settingService:SettingService,private sensorService:SensorService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getSensorSettingList();
    }

	ngOnInit() {

		this.getSettingList();
		this.getSensorList();
		this.createSensorSettingAddForm();
	}

	getSettingList(){
		this.settingService.getSettingList().subscribe(data=>{
			this.settingList=data;
		})
	}
	getSensorList(){
		this.sensorService.getSensorList().subscribe(data=>{
			this.sensorList=data;
		})
	}

	getSensorSettingList() {
		this.sensorSettingService.getSensorSettingList().subscribe(data => {
			this.sensorSettingList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.sensorSettingAddForm.valid) {
			this.sensorSetting = Object.assign({}, this.sensorSettingAddForm.value)

			if (this.sensorSetting.id == 0)
				this.addSensorSetting();
			else
				this.updateSensorSetting();
		}

	}

	addSensorSetting(){

		this.sensorSettingService.addSensorSetting(this.sensorSetting).subscribe(data => {
			this.getSensorSettingList();
			this.sensorSetting = new SensorSetting();
			jQuery('#sensorsetting').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.sensorSettingAddForm);

		})

	}

	updateSensorSetting(){

		this.sensorSettingService.updateSensorSetting(this.sensorSetting).subscribe(data => {

			var index=this.sensorSettingList.findIndex(x=>x.id==this.sensorSetting.id);
			this.sensorSettingList[index]=this.sensorSetting;
			this.dataSource = new MatTableDataSource(this.sensorSettingList);
            this.configDataTable();
			this.sensorSetting = new SensorSetting();
			jQuery('#sensorsetting').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.sensorSettingAddForm);

		})

	}

	createSensorSettingAddForm() {
		this.sensorSettingAddForm = this.formBuilder.group({		
			id : [0],
sensorId : [0, Validators.required],
settingId : [0, Validators.required]
		})
	}

	deleteSensorSetting(sensorSettingId:number){
		this.sensorSettingService.deleteSensorSetting(sensorSettingId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.sensorSettingList=this.sensorSettingList.filter(x=> x.id!=sensorSettingId);
			this.dataSource = new MatTableDataSource(this.sensorSettingList);
			this.configDataTable();
		})
	}

	getSensorSettingById(sensorSettingId:number){
		this.clearFormGroup(this.sensorSettingAddForm);
		this.sensorSettingService.getSensorSettingById(sensorSettingId).subscribe(data=>{
			this.sensorSetting=data;
			this.sensorSettingAddForm.patchValue(data);
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
