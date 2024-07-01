import createApiClient from "./api.service.js";
class CommentService {
    constructor(baseUrl = "http://localhost:5086/api/v1/comments") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async getAll(id) {
        return (await this.api.get(`${id}`, {
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }));
    }
    async addComment(data){
        return (await this.api.post("/add-comment",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
    async replyComment(data){
        return (await this.api.post("/reply-comment",data,{
            headers: {
                'Authorization': `Bearer ${this.token}`
            }
        }))
    }
}
export default new CommentService();