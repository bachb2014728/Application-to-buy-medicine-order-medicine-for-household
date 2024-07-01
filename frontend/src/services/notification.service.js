import createApiClient from "./api.service.js";
class NotificationService {
    constructor(baseUrl = "http://localhost:5086/api/v1/notifications") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async getAll() {
        return (await this.api.get("/", {
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
}
export default new NotificationService();