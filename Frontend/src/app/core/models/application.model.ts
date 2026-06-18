export interface Application {
 id: string; status: string; interviewNotes: string | null;
 rejectionReason: string | null; jobRoleId: string;
 createdAt: string; updatedAt: string | null;
}
export interface PipelineSummary { [status: string]: number; }