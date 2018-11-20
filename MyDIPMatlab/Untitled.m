clear;
clc;
%imageOrigin = imread('..\test\test5.jpg');
i=imread('..\test\test5.jpg');
i=rgb2gray(i);
%添加噪声
ig=imnoise(i,'poisson');
%获取算子
s=GetStrelList();
%串联去噪
edit erodeList
e=erodeList(ig,s);
edit getRateList
%计算权重
f=getRateList(ig,e);
edit getRemoveResult
%并联
igo=getRemoveResult(f,e);
%显示结果
subplot(1,2,1),imshow(f);
subplot(1,2,1),imshow(i);
title('原图像');
subplot(1,2,2),imshow(ig),title('噪声图像');
figure;
subplot(2,2,1),imshow(e.eroded_co12);title('串联1处理结果');
subplot(2,2,2),imshow(e.eroded_co22);title('串联2处理结果');
subplot(2,2,3),imshow(e.eroded_co32);title('串联3处理结果');
subplot(2,2,4),imshow(e.eroded_co42);title('串联4处理结果');
figure;
subplot(1,2,1),imshow(ig),title('噪声图像');
subplot(1,2,2),imshow(igo),title('并联去噪图像');
