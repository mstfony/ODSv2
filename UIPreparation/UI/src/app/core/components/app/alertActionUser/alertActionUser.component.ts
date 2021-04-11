import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { AlertActionUser } from './models/AlertActionUser';
import { AlertActionUserService } from './services/AlertActionUser.service';
import { environment } from 'environments/environment';
import { AlertAction } from '../alertAction/models/AlertAction';
import { User } from '../../admin/user/models/user';
import { AlertActionService } from '../alertAction/services/AlertAction.service';
import { UserService } from '../../admin/user/services/user.service';

declare var jQuery: any;

@Component({
	selector: 'app-alertActionUser',
	templateUrl: './alertActionUser.component.html',
	styleUrls: ['./alertActionUser.component.scss']
})
export class AlertActionUserComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','alertActionId','userId', 'update','delete'];

	alertActionUserList:AlertActionUser[];
	alertActionUser:AlertActionUser=new AlertActionUser();

	alertActionUserAddForm: FormGroup;


	alertActionUserId:number;

	alertActionList:AlertAction[];
	userList:User[];

	constructor(private alertActionUserService:AlertActionUserService,private alertActionService:AlertActionService,private userService:UserService ,private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getAlertActionUserList();
    }

	ngOnInit() {
		this.getAlertActionList();
		this.getUserList();
		this.createAlertActionUserAddForm();
	}

	getAlertActionList(){
		this.alertActionService.getAlertActionList().subscribe(data=>{
			this.alertActionList=data;
		})
	}

	getUserList(){
		this.userService.getUserList().subscribe(data=>{
			this.userList=data;
		})
	}

	getAlertActionUserList() {
		this.alertActionUserService.getAlertActionUserList().subscribe(data => {
			this.alertActionUserList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.alertActionUserAddForm.valid) {
			this.alertActionUser = Object.assign({}, this.alertActionUserAddForm.value)

			if (this.alertActionUser.id == 0)
				this.addAlertActionUser();
			else
				this.updateAlertActionUser();
		}

	}

	addAlertActionUser(){

		this.alertActionUserService.addAlertActionUser(this.alertActionUser).subscribe(data => {
			this.getAlertActionUserList();
			this.alertActionUser = new AlertActionUser();
			jQuery('#alertactionuser').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.alertActionUserAddForm);

		})

	}

	updateAlertActionUser(){

		this.alertActionUserService.updateAlertActionUser(this.alertActionUser).subscribe(data => {

			var index=this.alertActionUserList.findIndex(x=>x.id==this.alertActionUser.id);
			this.alertActionUserList[index]=this.alertActionUser;
			this.dataSource = new MatTableDataSource(this.alertActionUserList);
            this.configDataTable();
			this.alertActionUser = new AlertActionUser();
			jQuery('#alertactionuser').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.alertActionUserAddForm);

		})

	}

	createAlertActionUserAddForm() {
		this.alertActionUserAddForm = this.formBuilder.group({		
			id : [0],
alertActionId : [0, Validators.required],
userId : [0, Validators.required]
		})
	}

	deleteAlertActionUser(alertActionUserId:number){
		this.alertActionUserService.deleteAlertActionUser(alertActionUserId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.alertActionUserList=this.alertActionUserList.filter(x=> x.id!=alertActionUserId);
			this.dataSource = new MatTableDataSource(this.alertActionUserList);
			this.configDataTable();
		})
	}

	getAlertActionUserById(alertActionUserId:number){
		this.clearFormGroup(this.alertActionUserAddForm);
		this.alertActionUserService.getAlertActionUserById(alertActionUserId).subscribe(data=>{
			this.alertActionUser=data;
			this.alertActionUserAddForm.patchValue(data);
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
