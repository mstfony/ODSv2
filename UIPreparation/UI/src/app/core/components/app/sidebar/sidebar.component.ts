import { HostListener } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from '../../admin/login/services/auth.service';


declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
    claim:string;
}
export const ADMINROUTES: RouteInfo[] = [
  { path: '/user', title: 'Users', icon: 'how_to_reg', class: '', claim:"GetUsersQuery" },
  { path: '/group', title: 'Groups', icon:'groups', class: '',claim:"GetGroupsQuery" },
  { path: '/operationclaim', title: 'OperationClaim', icon:'local_police', class: '', claim:"GetOperationClaimsQuery"},
  { path: '/language', title: 'Languages', icon:'language', class: '', claim:"GetLanguagesQuery" },
  { path: '/translate', title: 'TranslateWords', icon: 'translate', class: '', claim: "GetTranslatesQuery" },
  { path: '/log', title: 'Logs', icon: 'update', class: '', claim: "GetLogDtoQuery" }
];

export const USERROUTES: RouteInfo[] = [ 
  { path: '/alert', title: 'Alert', icon: 'update', class: '', claim: "GetAlertQuery" },
  { path: '/alert-action', title: 'Alert Action', icon: 'update', class: '', claim: "GetAlertActionQuery" },
  { path: '/location', title: 'Location', icon: 'update', class: '', claim: "GetLocationQuery" },
  { path: '/parameter', title: 'Parameter', icon: 'update', class: '', claim: "GetParameterQuery" },
  { path: '/sensor', title: 'Sensor', icon: 'update', class: '', claim: "GetSensorQuery" },
  { path: '/sensor-location', title: 'Sensor Location', icon: 'update', class: '', claim: "GetSensorLocationQuery" },
  { path: '/sensor-setting', title: 'Sensor Settings', icon: 'update', class: '', claim: "GetSensorSettingQuery" },
  { path: '/sensor-value', title: 'Sensor Value', icon: 'update', class: '', claim: "GetSensorValueQuery" },
  { path: '/setting', title: 'Settings', icon: 'update', class: '', claim: "GetSettingQuery" },
  { path: '/alert-action-user', title: 'Alert Action User', icon: 'update', class: '', claim: "GetAlertActionUserQuery" },
  { path: '/alert-action-log', title: 'Alert Action Log', icon: 'update', class: '', claim: "GetAlertActionLogQuery" },
  { path: '/device', title: 'Device', icon: 'update', class: '', claim: "GetDeviceQuery" },
  { path: '/device-sensor', title: 'Device Sensor Settings', icon: 'update', class: '', claim: "GetDeviceSensorQuery" }
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  adminMenuItems: any[];
  userMenuItems: any[];

  constructor(private router:Router, private authService:AuthService,public translateService:TranslateService) {
    
  }

  ngOnInit() {
  
    this.adminMenuItems = ADMINROUTES.filter(menuItem => menuItem);
    this.userMenuItems = USERROUTES.filter(menuItem => menuItem);

    var lang=localStorage.getItem('lang') || 'tr-TR'
    this.translateService.use(lang);
  }
  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }
  ngOnDestroy() {
    if (!this.authService.loggedIn()) {
      this.authService.logOut();
      this.router.navigateByUrl("/login");
    }
  } 
 }

