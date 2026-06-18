export interface Referrer {
 id: string; name: string; linkedInUrl: string; designation: string;
 status: string; lastContactedAt: string | null; jobRoleId: string; createdAt: string;
}
export interface CreateReferrerRequest {
 name: string; linkedInUrl: string; designation: string; jobRoleId: string;
}