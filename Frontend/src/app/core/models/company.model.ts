export interface Company {
 id: string; name: string; careerPageUrl: string;
 tier: string; isBlacklisted: boolean; createdAt: string;
}
export interface CreateCompanyRequest { name: string; careerPageUrl: string; tier: 
string; }
