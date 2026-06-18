export interface JobRole {
 id: string; title: string; jobUrl: string; atsScore: number;
 isApplied: boolean; tier: string; companyId: string; createdAt: string;
}
export interface CreateJobRoleRequest {
 title: string; jobUrl: string; jobDescription: string; tier: string; companyId: string;
}