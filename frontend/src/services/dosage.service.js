import createApiClient from "./api.service.js";
class DosageService {
    constructor(baseUrl = "http://localhost:5086/api/v1/dosage-forms") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async getAll() {
        return (await this.api.get("/", {}));
    }
}
export default new DosageService();