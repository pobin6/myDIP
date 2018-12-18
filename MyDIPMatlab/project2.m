clc
clear 
A1=imread('..\project2\document1.bmp');
A1=rgb2gray(A1);
H=im2bw(A1);
J =~ H;
B = [true,true,false,false,false,false,true,true;true,false,false,true,true,false,false,true;true,false,true,true,true,false,false,true;false,false,true,true,true,true,true,true;false,false,true,true,true,true,true,true;true,false,false,true,true,true,false,true;true,false,false,false,true,false,false,true;true,true,false,false,false,false,true,true];
K=imerode(H,B);%用算子1去腐蚀原图
L=imerode(J,B);%用算子2去腐蚀原图的补集
K1=mat2gray(K);
L1=mat2gray(L);
R=mat2gray(H-K);%原图直接减去用算子1腐蚀的结果
M1=K-L;%击中击不中变换
M2=abs(K-L);%击中击不中变换的绝对值
R1=mat2gray(M1);
% R2=mat2gray(M2);%
R3=mat2gray(~M2);%击中击不中变换绝对值取反
figure(1);
subplot(231),imshow(H),title('原图灰度图');
subplot(232),imshow(K1);title('算子1腐蚀结果');
subplot(233),imshow(L1),title('算子2腐蚀结果');
subplot(234),imshow(R),title('原图直接减去腐蚀2结果');
subplot(235),imshow(R1),title('击中击不中变换结果');
subplot(236),imshow(R3),title('击中击不中变换取绝对值载取反结果');
% % 膨胀
% A2=imdilate(A1,B);
% figure
% imshow(A2);
% 腐蚀
% A3=imerode(A1,B);
% figure
% imshow(A3);
% % 开
% A4=imopen(A1,B);
% figure
% imshow(A4);
% % 闭
% A5=imclose(A1,B);
% figure
% imshow(A5);