import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Setting } from './models/Setting';
import { SettingService } from './services/Setting.service';
import { ParameterService } from '../parameter/services/parameter.service';
import { environment } from 'environments/environment';
import { Parameter } from '../parameter/models/Parameter';

declare var jQuery: any;

@Component({
	selector: 'app-setting',
	templateUrl: './setting.component.html',
	styleUrls: ['./setting.component.scss']
})
export class SettingComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','parameterId','value', 'update','delete'];

	settingList:Setting[];
	setting:Setting=new Setting();

	settingAddForm: FormGroup;

	parameterList:Parameter[];

	settingId:number;

	constructor(private settingService:SettingService, private parameterService : ParameterService ,private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getSettingList();
    }

	ngOnInit() {

		this.getParameterList();
		this.createSettingAddForm();
	}

	getParameterList(){
		this.parameterService.getParameterList().subscribe(data=>{
			this.parameterList=data;
		})
	}

	getSettingList() {
		this.settingService.getSettingList().subscribe(data => {
			this.settingList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.settingAddForm.valid) {
			this.setting = Object.assign({}, this.settingAddForm.value)

			if (this.setting.id == 0)
				this.addSetting();
			else
				this.updateSetting();
		}

	}

	addSetting(){

		this.settingService.addSetting(this.setting).subscribe(data => {
			this.getSettingList();
			this.setting = new Setting();
			jQuery('#setting').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.settingAddForm);

		})

	}

	updateSetting(){

		this.settingService.updateSetting(this.setting).subscribe(data => {

			var index=this.settingList.findIndex(x=>x.id==this.setting.id);
			this.settingList[index]=this.setting;
			this.dataSource = new MatTableDataSource(this.settingList);
            this.configDataTable();
			this.setting = new Setting();
			jQuery('#setting').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.settingAddForm);

		})

	}

	createSettingAddForm() {
		this.settingAddForm = this.formBuilder.group({		
			id : [0],
parameterId : [0, Validators.required],
value : [0, Validators.required]
		})
	}

	deleteSetting(settingId:number){
		this.settingService.deleteSetting(settingId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.settingList=this.settingList.filter(x=> x.id!=settingId);
			this.dataSource = new MatTableDataSource(this.settingList);
			this.configDataTable();
		})
	}

	getSettingById(settingId:number){
		this.clearFormGroup(this.settingAddForm);
		this.settingService.getSettingById(settingId).subscribe(data=>{
			this.setting=data;
			this.settingAddForm.patchValue(data);
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
