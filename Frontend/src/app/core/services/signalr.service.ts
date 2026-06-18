import { Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
export interface ReminderAlert { message: string; referrerId: string; jobRoleId: 
string; }
@Injectable({ providedIn: 'root' })
export class SignalRService {
 private hub!: signalR.HubConnection;
 reminder = signal<ReminderAlert | null>(null);
 connect(token: string) {
 this.hub = new signalR.HubConnectionBuilder()
 .withUrl('https://localhost:7288/hubs/reminders', {
 accessTokenFactory: () => token
 })
 .withAutomaticReconnect()
 .build();
 this.hub.on('FollowUpReminder', (data: ReminderAlert) => {
 this.reminder.set(data); // signal update — navbar toast appears automatically
 });
 this.hub.start().catch(err => console.error('SignalR error:', err));
 }
 disconnect() { this.hub?.stop(); }
}